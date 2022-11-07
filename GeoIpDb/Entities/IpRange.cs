using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Entities
{
    public class IpRange
    {
        public string IpRangeStart { get; set; }

        public string IpRangeEnd { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null|| !this.GetType().Equals(obj.GetType())) return false;
            IpRange other = (IpRange)obj;

            return (this.IpRangeStart==other.IpRangeStart&&this.IpRangeEnd==other.IpRangeEnd);
        }
    }
}
