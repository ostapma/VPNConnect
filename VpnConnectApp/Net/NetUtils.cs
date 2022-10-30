using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VPNConnect.Net
{

    public static class NetUtils
    {
        public static long IpToInt(string ipString)
        {
            var ipNum = ipString.Split('.');

            long result = 0;

            int bitshift = 0;

            foreach (var ipN in ipNum.Reverse())
            {
                long n;
                if (long.TryParse(ipN, out n))
                {
                    result += n << bitshift;
                    bitshift += 8;
                }
                else return 0;
            }

            return result;
        }

    }
}
