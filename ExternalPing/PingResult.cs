using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalPing
{
    public class PingResult
    {
            public DateTime PingTime { get; set; }
            public int PingLatency { get; set; }
            public bool IsSuccess
            {
                get; set;
            }

            public string Error { get; set; }

            public string Ip { get; set; }
    }
}
