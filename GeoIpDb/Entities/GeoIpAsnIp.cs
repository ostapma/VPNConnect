using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Entities
{
    public class GeoIpAsnIp
    {
        public int AsnIpId { get; set; }

        public IpRange IpRange { get; set; }        
        public GeoIpAsn Asn { get; set; }


        public bool IsBlacklisted { get; set; }
    }
}
