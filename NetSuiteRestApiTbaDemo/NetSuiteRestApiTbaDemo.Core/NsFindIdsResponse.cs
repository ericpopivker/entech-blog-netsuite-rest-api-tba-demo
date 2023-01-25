using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSuiteRestApiTbaDemo.Core
{
    public class NsFindIdsResponse
    {
        public int count { get; set; }

        public List<Item> items { get; set; }

        public class Item
        {
            public string id { get; set; }
        }
    }
}
