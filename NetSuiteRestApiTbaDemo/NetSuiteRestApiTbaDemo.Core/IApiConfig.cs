using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSuiteRestApiTbaDemo.Core
{
    public interface IApiConfig
    {
        public string AccountId { get; }

        public string ClientId { get; }
        public string ClientSecret { get; }

        public string TokenId { get; }
        public string TokenSecret { get; }
        
        public string ApiRoot { get; }

    }
}
