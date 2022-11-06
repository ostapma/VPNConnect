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
        private string currentIp;
        ExternalIpServiceProvider externalIpServiceProvider;


        public VpnSearcher(IVpnUiHandler vpnUiHandler , VpnSearchSettings settings
            )
        {
            this.vpnUiHandler = vpnUiHandler;
            this.settings = settings;
            externalIpServiceProvider = new ExternalIpServiceProvider(settings.ExternalIpServiceLink);
        }

        public void Start()
        {
            var disconnectedExternalIp = externalIpServiceProvider.GetMyIp();
            if (!disconnectedExternalIp.IsSuccess)
            {
                Log.Error($"Can't get current connection info, check your Internet connection");
                return;
            }
            Log.Information($"My IP is {disconnectedExternalIp.IpAddress}");
            var geoIpRepository = new GeoIp.Repo.GeoIpCityRepository(settings.GeoIpDbSettings.ConnectionString);
            keyboardHookManager.Start();

            keyboardHookManager.RegisterHotkey(GetVcode(settings.ConsoleSettings.StopHotKey), () =>
            {
                if (isStarted)
                {
                    Log.Information($"{settings.ConsoleSettings.StopHotKey} pressed");
                    Log.Information("VPN searching is stopping");
                    isStarted = false;
                }
                
            });

            keyboardHookManager.RegisterHotkey(GetVcode(settings.ConsoleSettings.StartHotKey), () =>
            {
                if (isStarted) return;
                isStarted = true;
                Log.Information($"{settings.ConsoleSettings.StartHotKey} pressed");
                Log.Information("VPN searching is started");

                NetQualityAnalyzer netQualityAnalyzer = new(settings.NetAnanlyzeSettings.PingTarget,
                    settings.NetAnanlyzeSettings.PingHops, settings.NetAnanlyzeSettings.BlacklistCountries);

                while (isStarted)
                {
                    try {

                        Log.Information("Emulate mouse left click on VPN client CONNECT button");

                        vpnUiHandler.PressConnect();

                        Log.Information($"Waiting for VPN connection {settings.VpnUiHandlingSettings.ConnectTimeoutSec} sec");

                        bool isVpnStateChanged = IsVpnStateChanged(disconnectedExternalIp.IpAddress);


                        if (!IsVpnStateChanged(disconnectedExternalIp.IpAddress))
                        {
                            Log.Information($"Can't connect, VPN client seems doesn't work");
                            isStarted = false;
                        }
                        else
                        {
                            Log.Information($"Connected. My IP: {currentIp}");

                            var geoiInfo = geoIpRepository.GetByIpAddress(currentIp);

                            if (geoiInfo != null) {
                                Log.Information($"The VPN geoip info: country code: {geoiInfo.CountryID}; city: {geoiInfo.CityName}");
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
                                Disconect();
                            }
                            else
                            {
                                Log.Information($"Started network quality analyzing with {settings.NetAnanlyzeSettings.PingTarget} as target");
                                netQuality = netQualityAnalyzer.Analyze(settings.NetAnanlyzeSettings.TolerablePacketLoss);
                                if (netQuality.IsValid)
                                {
                                    Log.Information($"Packets lost: {netQuality.LostPackets}; avg latency: {netQuality.AvgLatency}");
                                    if (netQuality.LostPackets < settings.NetAnanlyzeSettings.TolerablePacketLoss
                                        && netQuality.AvgLatency < settings.NetAnanlyzeSettings.TolerableLatencySec)
                                    {
                                        Log.Information("The VPN is good enough, let's stop here");
                                        isStarted = false;
                                    }
                                    else
                                    {
                                        Log.Information("The VPN connection is no good");
                                        Disconect();
                                    }
                                }
                                else
                                {
                                    Log.Information("Latency analyzing was unsuccesfull");
                                    Disconect();
                                }
                                 
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


        private bool IsVpnStateChanged(string initialIp)
        {
            int connectionTimeSec = 0;
            while (connectionTimeSec < settings.VpnUiHandlingSettings.ConnectTimeoutSec)
            {
                Thread.Sleep(SecToMs(1));
                connectionTimeSec++;
                var ip = externalIpServiceProvider.GetMyIp();
                if (ip.IsSuccess && ip.IpAddress!=initialIp)
                {
                    currentIp = ip.IpAddress;
                    return true;
                }
            }
            return false;

        }

        private void Disconect()
        {
            Log.Information("Disconnecting");
            Log.Information("Simulate mouse left click on VPN client DISCONNECT button");
            vpnUiHandler.PressDisconnect();
            Log.Information($"Waiting for disconnect {settings.VpnUiHandlingSettings.ConnectTimeoutSec} sec");

            if (!IsVpnStateChanged(currentIp))
            {
                Log.Error("Can't disconnect, there is something wrong with your VPN client");
                isStarted = false;
            }
        }

        private int GetVcode(string code)
        {
            return (int)Enum.Parse(typeof(Keys), code);
        }

        private int SecToMs(int sec)
        {
            return sec * 1000;
        }

        public void Stop()
        {
            isStarted = false;
            keyboardHookManager.UnregisterHotkey(GetVcode(settings.ConsoleSettings.StopHotKey));
            keyboardHookManager.UnregisterHotkey(GetVcode(settings.ConsoleSettings.StartHotKey));
        }
    }
}
