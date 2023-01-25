
namespace NetSuiteRestApiTbaDemo.Core
{
    public class NetSuiteApiConfig : IApiConfig
    {
        public string AccountId { get; } =  "<Enter NetSuite AccountID>";
        
        public string ClientId { get; } = "<Enter Integration ClientId>";
        public string ClientSecret { get; } = "<Enter Integration ClientSecret>";

        public string TokenId { get; } = "<Enter Access TokenId>";
        public string TokenSecret { get; } = "<Enter Access TokenSecret>";

        public string ApiRoot { get; } = $"https://<Account Prefix>.suitetalk.api.netsuite.com/services/rest/record/v1";

    }
}