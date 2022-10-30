using NonInvasiveKeyboardHookLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Net.NetworkInformation;
using VPNConnect.UIHandling;
using VPNConnect.Net;

namespace VPNConnect
{
    internal class VpnSearcher
    {
        bool isStarted = false;
        KeyboardHookManager keyboardHookManager = new();
        private readonly IVpnUiHandler vpnUiHandler;
        private readonly ConsoleSettings consoleSettings;
        private readonly VpnUiHandlingSettings vpnUiHandlingSettings;
        private readonly NetAnanlyzeSettings netAnanlyzeSettings;
        private readonly GeoIpDbSettings geoIpDbSettings;
        private readonly string externalIpServiceLink;
        private string disconnectedExternalIp = "";


        public VpnSearcher(IVpnUiHandler vpnUiHandler , ConsoleSettings consoleSettings,
            VpnUiHandlingSettings vpnUiHandlingSettings, NetAnanlyzeSettings netAnanlyzeSettings,
            GeoIpDbSettings geoIpDbSettings, string externalIpServiceLink
            )
        {
            this.vpnUiHandler = vpnUiHandler;
            this.consoleSettings = consoleSettings;
            this.vpnUiHandlingSettings = vpnUiHandlingSettings;
            this.netAnanlyzeSettings = netAnanlyzeSettings;
            this.geoIpDbSettings = geoIpDbSettings;
            this.externalIpServiceLink = externalIpServiceLink;
        }

        public void StartHotkey()
        {
            ExternalIpServiceProvider externalIpServiceProvider = new ExternalIpServiceProvider(externalIpServiceLink);
            Log.Information($"My IP is {disconnectedExternalIp}");
            var geoIpRepository = new GeoIp.GeoIpRepository(geoIpDbSettings.ConnectionString);
            keyboardHookManager.Start();

            keyboardHookManager.RegisterHotkey(GetVcode(consoleSettings.StopHotKey), () =>
            {
                Log.Information($"{consoleSettings.StopHotKey} pressed");
                Log.Information("VPN searching is stopping");
                isStarted = false;
            });

            keyboardHookManager.RegisterHotkey(GetVcode(consoleSettings.StartHotKey), () =>
            {
                if (isStarted) return;
                Log.Information($"{consoleSettings.StartHotKey} pressed");
                Log.Information("VPN searching is started");

                NetQualityAnalyzer netQualityAnalyzer = new(netAnanlyzeSettings.PingTarget, netAnanlyzeSettings.PingHops, netAnanlyzeSettings.BlacklistCountries);

                isStarted = true;
                while (isStarted)
                {
                    try {

                        Log.Information("Emulate mouse left click on VPN client CONNECT button");

                        vpnUiHandler.PressConnect();

                        Log.Information($"Waiting for VPN connection {vpnUiHandlingSettings.ConnectTimeoutSec} sec");

                        int connectionTimeSec = 0;
                        string connectedExternalIp = disconnectedExternalIp;
                        while(connectionTimeSec< vpnUiHandlingSettings.ConnectTimeoutSec && connectedExternalIp== disconnectedExternalIp)
                        {
                            Thread.Sleep(SecToMs(1));
                            connectionTimeSec++;
                            try
                            {
                                connectedExternalIp = externalIpServiceProvider.GetMyIp();
                            }
                            catch (HttpRequestException ex)
                            {
                                Log.Debug($"Can't connect to myIp service {ex}");
                            }
                            catch (AggregateException ex )
                            {
                                if (ex.InnerException is HttpRequestException)
                                    Log.Debug($"Can't connect to myIp service {ex}");
                                else throw;
                            }
                            
                        }
                        
                        if (connectedExternalIp == disconnectedExternalIp)
                        {
                            Log.Information($"Can't connect, VPN client seems doesn't work");
                            isStarted = false;
                        }
                        else
                        {
                            Log.Information($"Connected. My IP: {connectedExternalIp}");

                            var geoiInfo = geoIpRepository.GetByIpAddress(NetUtils.IpToInt(connectedExternalIp));

                            if (geoiInfo != null) {
                                Log.Information($"The VPN geoip info: countryID: {geoiInfo.CountryID} city: {geoiInfo.CityName}");
                            }
                            else
                            {
                                Log.Information($"The VPN geoip info is not found");
                            }

                            bool isCountryBlacklisted = netQualityAnalyzer.IsCountryBlacklisted(geoiInfo != null ? geoiInfo.CountryID : "");
                            NetQuality? netQuality = null;

                            if (isCountryBlacklisted)
                            {
                                Log.Information($"The VPN country is in your blacklist");
                            }
                            else
                            {
                                Log.Information($"Started network quality analyzing with {netAnanlyzeSettings.PingTarget} as target");
                                netQuality = netQualityAnalyzer.Analyze(netAnanlyzeSettings.TolerablePacketLoss);
                                if (netQuality.IsValid)
                                    Log.Information($"Packets lost: {netQuality.LostPackets}; avg latency: {netQuality.AvgLatency}");
                                else
                                    Log.Information("Latency analyzing was unsuccesfull");
                            }

                            if (isCountryBlacklisted
                                || netQuality==null
                                || !netQuality.IsValid 
                                || netQuality.LostPackets > netAnanlyzeSettings.TolerablePacketLoss 
                                || netQuality.AvgLatency > netAnanlyzeSettings.TolerableLatencySec)
                            {
                                Log.Information("The VPN is no good");
                                Log.Information("Disconnecting");
                                Log.Information("Simulate mouse left click on VPN client DISCONNECT button");
                                vpnUiHandler.PressDisconnect();
                                Log.Information($"Waiting for disconnect {vpnUiHandlingSettings.ConnectTimeoutSec} sec");
                                Thread.Sleep(SecToMs(vpnUiHandlingSettings.ConnectTimeoutSec));

                            }
                            else
                            {
                                Log.Information("The VPN is good, let's stop");
                                isStarted = false;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error on VPN Search: {ex}");
                        isStarted = false;
                    }
                }

                Log.Information("VPN searching has stopped");

            });
        }

        private int GetVcode(string code)
        {
            return (int)Enum.Parse(typeof(Keys), code);
        }

        private int SecToMs(int sec)
        {
            return sec * 1000;
        }

        public void StopHotkey()
        {
            isStarted = false;
            keyboardHookManager.UnregisterHotkey(GetVcode(consoleSettings.StopHotKey));
            keyboardHookManager.UnregisterHotkey(GetVcode(consoleSettings.StartHotKey));
        }
    }
}
