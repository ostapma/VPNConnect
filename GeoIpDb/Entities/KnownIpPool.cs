using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Entities
{
    public class KnownIpPool
    {
        public IpRange IpRange { get; set; }

        public DateTime DateAdded { get; set; }

        public string Comments { get; set; }
        public bool IsBlacklisted { get; set; }

        public bool IsGood { get; set; }

        public int? ApplicationId { get; set; }

    }
}
