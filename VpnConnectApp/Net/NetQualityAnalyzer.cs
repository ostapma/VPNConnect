﻿using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Log = Serilog.Log;

namespace VPNConnect.Net
{
    internal class NetQuality
    {
        public long AvgLatency { get; set; } = 0;
        public int LostPackets { get; set; } = 0;

        public bool IsValid { get; set; } = false;
    }

    internal class NetQualityAnalyzer
    {
        int retries;
        private readonly List<string> blacklistCountries;
        string pingTarget;


        public NetQualityAnalyzer(string pingTarget, int retries, 
            List<string> blacklistCountries)
        {
            this.pingTarget = pingTarget;
            this.retries = retries;
            this.blacklistCountries = blacklistCountries;
        }

        public bool IsCountryBlacklisted(string connectionIpCountry)
        {
            return blacklistCountries.Any(c => c.ToLower() == connectionIpCountry.ToLower());

        }

        public NetQuality Analyze(int lostPacketsLimit)
        {
            var quality = new NetQuality();
            long totalLatency = 0;
            Ping ping = new Ping();
            int completedTries = 0;
            try
            {
                while (completedTries < retries && quality.LostPackets<=lostPacketsLimit)
                {
                    PingReply? result;
                    result = ping.Send(pingTarget);

                    if (result == null || result.Status != IPStatus.Success)
                    {
                        quality.LostPackets++;
                    }
                    else
                    {
                        totalLatency += result.RoundtripTime;
                    }
                    completedTries++;
                }                
                if (quality.LostPackets < retries) quality.AvgLatency = totalLatency / (retries - quality.LostPackets);
                quality.IsValid = true;

            }
            catch (PingException pe)
            {
                Log.Debug($"Ping error: {pe}");
                quality.IsValid = false;
            }

            return quality;
        }


    }
}
