using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrusterWebApi.Model
{
    public class ComputerInfoModel
    {
        public int ComputerID { get; set; }
        public string ComputerName { get; set; }
        public int OSTypeID { get; set; }
        public string OSTypeName { get; set; }
        public string ComputerModel { get; set; }
        public bool ComputerIsMobile { get; set; }
        public string ComputerModelManufacturer { get; set; }
        public string ComputerBiosManufacturer { get; set; }
        public string OUName { get; set; }
    }
}
