using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIp.Entities
{
    public class GeoIpCity
    {
        public long CityIpId { get; set; }  

        public string IpRangeStart { get; set; }

        public string IpRangeEnd { get; set; }

        public string RegionName { get; set; }
        public string CityName { get; set; }    
        public string CountryID { get; set; }

        public string ContinentID { get; set; }

    }
}
