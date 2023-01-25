using NUnit.Framework;
using System.Threading.Tasks;

using NetSuiteRestApiTbaDemo.Core;

namespace NetSuiteRestApiTbaDemo.Core.Tests
{
    public class NetSuiteApiClient_UsingHttpClient_Tests
    {

        [Test]
        public async Task FindCustomerIds_LimitTwo_TwoIds()
        {
            var nsApiClient = new NetSuiteApiClient_UsingHttpClient();
            var ids = await nsApiClient.FindCustomerIds(2);
            Assert.AreEqual(2, ids.Count);
        }

        [Test]
        public async Task GetCustomer_ValidId_ReturnsCustomer()
        {
            var nsApiClient = new NetSuiteApiClient_UsingHttpClient();
            var customer = await nsApiClient.GetCustomer(125173);

            Assert.IsNotNull(customer);
        }
    }
}