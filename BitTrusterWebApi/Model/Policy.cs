using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrusterWebApi.Model
{
    public class Policy
    {
        public int PolicyID { get; set; }
        public string Name { get; set; }
        public string TableColumnName { get; set; }
        public string TableColumeValue { get; set; }
        public Dictionary<string, List<string>> PoliciesFilters { get; set; }
    }
}
