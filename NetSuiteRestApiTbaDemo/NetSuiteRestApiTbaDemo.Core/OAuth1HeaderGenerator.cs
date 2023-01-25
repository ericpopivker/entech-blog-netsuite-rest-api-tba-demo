using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Specialized;

namespace NetSuiteRestApiTbaDemo.Core
{
    public class OAuth1HeaderGenerator
    {
        private IApiConfig _config = null;
        
        private HttpMethod _httpMethod;
        private string _requestUrl;

        public OAuth1HeaderGenerator(IApiConfig apiConfig, HttpMethod httpMethod, string requestUrl)
        {
            _config = apiConfig;
            _httpMethod= httpMethod;
            _requestUrl= requestUrl;
        }

        public AuthenticationHeaderValue CreateAuthenticationHeaderValue()
        {
            var nonce = GenerateNonce();
            var timestamp = GenerateTimeStamp();

            var parameter = GetAuthenticationHeaderValueParameter(nonce, timestamp);
            return new AuthenticationHeaderValue("OAuth", parameter);
        }

        public string GetAuthenticationHeaderValueParameter(string nonce, string timestamp)
        {
            var signature = GenerateSignature(nonce, timestamp);

            //NetSuite doc:
            //https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941295.html

            var parameters = new OrderedDictionary
            {
                { "realm", _config.AccountId },
                { "oauth_token", _config.TokenId },
                { "oauth_consumer_key", _config.ClientId },
                { "oauth_nonce", nonce },
                { "oauth_timestamp", timestamp },
                { "oauth_signature_method", "HMAC-SHA256" },
                { "oauth_version", "1.0" },
                { "oauth_signature", signature }
            };

            var combinedOauthParams = CombineOAuthHeaderParams(parameters);
            Debug.WriteLine($"CreateAuthenticationHeaderValue: combinedOauthParams: {combinedOauthParams}");

            return combinedOauthParams;
        }


        private string CombineOAuthHeaderParams(OrderedDictionary parameters)
        {
            var sb = new StringBuilder();
            var first = true;

            foreach (var key in parameters.Keys)
            {
                if (!first)
                    sb.Append(", ");

                var value = parameters[key].ToString();
                value = Uri.EscapeDataString(value);
                sb.Append($"{key}=\"{value}\"");
                first = false;
            }

            return sb.ToString();
        }


        // From NetSuite doc:
        // https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941088.html
        public string GenerateSignature(string nonce, string timestamp)
        {
            var baseString = GenerateSignatureBaseString(nonce, timestamp);           
            string key = GenerateSignatureKey();

            Debug.WriteLine($"GenerateSignature: baseString: {baseString}");
            Debug.WriteLine($"GenerateSignature: key: {key}");

            string signature = "";

            var encoding = new ASCIIEncoding();

            byte[] keyBytes = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(baseString);

            using (var hmaCsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hash = hmaCsha256.ComputeHash(messageBytes);
                signature = Convert.ToBase64String(hash);
            }
            
            return signature;
        }

        public string GenerateSignatureKey()
        {
            var keyParams = new List<string>
                {
                    _config.ClientSecret,
                    _config.TokenSecret
                };

            var key = CombineKeyParams(keyParams);
            return key;
        }

        public string GenerateSignatureBaseString(string nonce, string timestamp)
        {
            var requestUri = new Uri(_requestUrl);
            var requestUrlPath = requestUri.GetLeftPart(UriPartial.Path);
            
            string baseString = _httpMethod.ToString();
            baseString += "&";
            baseString += Uri.EscapeDataString(requestUrlPath);
            baseString += "&";

            //Handle query string
            var requestUriQuery = requestUri.Query;
            if (requestUriQuery != string.Empty)
            {
                //Remove '?'
                requestUriQuery = requestUriQuery.Remove(0, 1);
                baseString += Uri.EscapeDataString(requestUriQuery);
                baseString += Uri.EscapeDataString("&"); 
            }

            var baseStringParams = new OrderedDictionary()
            {
                { "oauth_consumer_key", _config.ClientId },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA256" },
                { "oauth_timestamp", timestamp },
                { "oauth_token", _config.TokenId },
                { "oauth_version", "1.0" }
            };

            baseString += CombineBaseStringParams(baseStringParams);
            return baseString;
        }

        private string CombineBaseStringParams(OrderedDictionary parameters)
        {
            var sb = new StringBuilder();
            var first = true;
            var separator = Uri.EscapeDataString("&");

            foreach (var key in parameters.Keys)
            {
                if (!first)
                    sb.Append(separator);

                var value = parameters[key];
                var keyAndValue = Uri.EscapeDataString($"{key}={value}");
                sb.Append(keyAndValue);

                first = false;
            }

            return sb.ToString();
        }

        private string CombineKeyParams(List<string> parameters)
        {
            var sb = new StringBuilder();
            var first = true;

            foreach (var param in parameters)
            {
                if (!first)
                    sb.Append("&");

                sb.Append(Uri.EscapeDataString(param));
                first = false;
            }

            return sb.ToString();
        }

        public string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public string GenerateNonce()
        {
            var random = new Random();

            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(123400, 9999999).ToString();
        }
    }
}
