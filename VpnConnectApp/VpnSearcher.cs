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
using VpnConnect.Configuration;

namespace VPNConnect
{
    internal class VpnSearcher
    {
        bool isStarted = false;
        KeyboardHookManager keyboardHookManager = new();
        private readonly IVpnUiHandler vpnUiHandler;
        private readonly VpnSearchSettings settings;
        private string disconnectedExternalIp = "";


        public VpnSearcher(IVpnUiHandler vpnUiHandler , VpnSearchSettings settings
            )
        {
            this.vpnUiHandler = vpnUiHandler;
            this.settings = settings;
        }

        public void StartHotkey()
        {
            ExternalIpServiceProvider externalIpServiceProvider = new ExternalIpServiceProvider(settings.ExternalIpServiceLink );
            disconnectedExternalIp = externalIpServiceProvider.GetMyIp();
            Log.Information($"My IP is {disconnectedExternalIp}");
            var geoIpRepository = new GeoIp.Repo.GeoIpRepository(settings.GeoIpDbSettings.ConnectionString);
            keyboardHookManager.Start();

            keyboardHookManager.RegisterHotkey(GetVcode(settings.ConsoleSettings.StopHotKey), () =>
            {
                Log.Information($"{settings.ConsoleSettings.StopHotKey} pressed");
                Log.Information("VPN searching is stopping");
                isStarted = false;
            });

            keyboardHookManager.RegisterHotkey(GetVcode(settings.ConsoleSettings.StartHotKey), () =>
            {
                if (isStarted) return;
                Log.Information($"{settings.ConsoleSettings.StartHotKey} pressed");
                Log.Information("VPN searching is started");

                NetQualityAnalyzer netQualityAnalyzer = new(settings.NetAnanlyzeSettings.PingTarget,
                    settings.NetAnanlyzeSettings.PingHops, settings.NetAnanlyzeSettings.BlacklistCountries);

                isStarted = true;
                while (isStarted)
                {
                    try {

                        Log.Information("Emulate mouse left click on VPN client CONNECT button");

                        vpnUiHandler.PressConnect();

                        Log.Information($"Waiting for VPN connection {settings.VpnUiHandlingSettings.ConnectTimeoutSec} sec");

                        int connectionTimeSec = 0;
                        string connectedExternalIp = disconnectedExternalIp;
                        while(connectionTimeSec< settings.VpnUiHandlingSettings.ConnectTimeoutSec && connectedExternalIp== disconnectedExternalIp)
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

                            var geoiInfo = geoIpRepository.GetByIpAddress(connectedExternalIp);

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
                                Log.Information($"Started network quality analyzing with {settings.NetAnanlyzeSettings.PingTarget} as target");
                                netQuality = netQualityAnalyzer.Analyze(settings.NetAnanlyzeSettings.TolerablePacketLoss);
                                if (netQuality.IsValid)
                                    Log.Information($"Packets lost: {netQuality.LostPackets}; avg latency: {netQuality.AvgLatency}");
                                else
                                    Log.Information("Latency analyzing was unsuccesfull");
                            }

                            if (isCountryBlacklisted
                                || netQuality==null
                                || !netQuality.IsValid 
                                || netQuality.LostPackets > settings.NetAnanlyzeSettings.TolerablePacketLoss 
                                || netQuality.AvgLatency > settings.NetAnanlyzeSettings.TolerableLatencySec)
                            {
                                Log.Information("The VPN is no good");
                                Log.Information("Disconnecting");
                                Log.Information("Simulate mouse left click on VPN client DISCONNECT button");
                                vpnUiHandler.PressDisconnect();
                                Log.Information($"Waiting for disconnect {settings.VpnUiHandlingSettings.ConnectTimeoutSec} sec");
                                Thread.Sleep(SecToMs(settings.VpnUiHandlingSettings.ConnectTimeoutSec));

                            }
                            else
                            {
                                Log.Information("The VPN is good enough, let's stop here");
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
            keyboardHookManager.UnregisterHotkey(GetVcode(settings.ConsoleSettings.StopHotKey));
            keyboardHookManager.UnregisterHotkey(GetVcode(settings.ConsoleSettings.StartHotKey));
        }
    }
}
