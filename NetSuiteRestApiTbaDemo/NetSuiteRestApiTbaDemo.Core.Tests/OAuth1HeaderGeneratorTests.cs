using System;
using System.Net.Http;
using NUnit.Framework;


namespace NetSuiteRestApiTbaDemo.Core.Tests
{
    public class OAuth1HeaderGeneratorTests
    {
        private string _nonce = "fjaLirsIcCGVZWzBX0pg";
        private string _timestamp = "1508242306";
        private string _requestUrl = "https://123456.suitetalk.api.netsuite.com/services/rest/record/v1/employee/40";

        private OAuth1HeaderGenerator CreateOAuth1HeaderGenerator()
        {
            var requestUrl = _requestUrl;

            return new OAuth1HeaderGenerator(new TestApiConfig(), HttpMethod.Get, requestUrl);
        }

        [Test]
        public void GenerateSignatureBaseString_FromNetSuiteDocExample_Works()
        {
            //From NetSuite documentation: https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941088.html

            var oauth1 = CreateOAuth1HeaderGenerator();
            var baseString = oauth1.GenerateSignatureBaseString(_nonce, _timestamp);

            Assert.AreEqual("GET&https%3A%2F%2F123456.suitetalk.api.netsuite.com%2Fservices%2Frest%2Frecord%2Fv1%2Femployee%2F40&oauth_consumer_key%3Def40afdd8abaac111b13825dd5e5e2ddddb44f86d5a0dd6dcf38c20aae6b67e4%26oauth_nonce%3DfjaLirsIcCGVZWzBX0pg%26oauth_signature_method%3DHMAC-SHA256%26oauth_timestamp%3D1508242306%26oauth_token%3D2b0ce516420110bcbd36b69e99196d1b7f6de3c6234c5afb799b73d87569f5cc%26oauth_version%3D1.0", baseString);
        }

        [Test]
        public void GenerateSignatureKey_FromNetSuiteDocExample_Works()
        {
            //From NetSuite documentation: https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941088.html

            var oauth1 = CreateOAuth1HeaderGenerator();
            var key = oauth1.GenerateSignatureKey();

            Assert.AreEqual("d26ad321a4b2f23b0741c8d38392ce01c3e23e109df6c96eac6d099e9ab9e8b5&c29a677df7d5439a458c063654187e3d678d73aca8e3c9d8bea1478a3eb0d295", key);
        }


        [Test]
        public void GenerateSignature_FromNetSuiteDocExample_Works()
        {
            //From NetSuite documentation: https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941088.html

            var oauth1 = CreateOAuth1HeaderGenerator();
            var signature = oauth1.GenerateSignature(_nonce, _timestamp);

            //for some reason oracle shows Url Encoded signature in their example
            //https://www.screencast.com/t/yXu08Brr
            signature = Uri.EscapeDataString(signature);
            Assert.AreEqual("B5OIWznZ2YP0OB7VrJrGkYsTh%2B8H%2B5T9Hag%2Bo92q0zY%3D", signature);
        }

        [Test]
        public void GetAuthenticationHeaderValueParameter_FromNetSuiteDocExample_Works()
        {
            //From NetSuite documentation: https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941295.html
 
            var oauth1 = CreateOAuth1HeaderGenerator();
            var parameter = oauth1.GetAuthenticationHeaderValueParameter(_nonce, _timestamp);

            string expected = "realm=\"123456\", oauth_token=\"2b0ce516420110bcbd36b69e99196d1b7f6de3c6234c5afb799b73d87569f5cc\", oauth_consumer_key=\"ef40afdd8abaac111b13825dd5e5e2ddddb44f86d5a0dd6dcf38c20aae6b67e4\", oauth_nonce=\"fjaLirsIcCGVZWzBX0pg\", oauth_timestamp=\"1508242306\", oauth_signature_method=\"HMAC-SHA256\", oauth_version=\"1.0\", oauth_signature=\"B5OIWznZ2YP0OB7VrJrGkYsTh%2B8H%2B5T9Hag%2Bo92q0zY%3D\"";
            Assert.AreEqual(expected, parameter);
        }
    }
}