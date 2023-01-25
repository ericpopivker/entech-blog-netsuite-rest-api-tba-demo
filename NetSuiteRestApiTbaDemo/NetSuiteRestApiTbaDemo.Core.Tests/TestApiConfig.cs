
namespace NetSuiteRestApiTbaDemo.Core.Tests
{
    public class TestApiConfig : IApiConfig
    {
        // From NetSuite documentation
        // https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941088.html
        public string AccountId { get; } = "123456";

        public string ClientId { get; } = "ef40afdd8abaac111b13825dd5e5e2ddddb44f86d5a0dd6dcf38c20aae6b67e4";
        public string ClientSecret { get; } = "d26ad321a4b2f23b0741c8d38392ce01c3e23e109df6c96eac6d099e9ab9e8b5";

        public string TokenId { get; } = "2b0ce516420110bcbd36b69e99196d1b7f6de3c6234c5afb799b73d87569f5cc";
        public string TokenSecret { get; } = "c29a677df7d5439a458c063654187e3d678d73aca8e3c9d8bea1478a3eb0d295";

        public string ApiRoot { get; } = $"https://123456.suitetalk.api.netsuite.com/services/rest/record/v1";
    }
}