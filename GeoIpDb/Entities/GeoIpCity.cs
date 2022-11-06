using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Entities
{
    public class GeoIpCity
    {
        public long CityIpId { get; set; }  

        public IpRange IpRange { get; set; }
        
        public string RegionName { get; set; }
        public string CityName { get; set; }    
        public string CountryID { get; set; }

        public string ContinentID { get; set; }

    }
}
