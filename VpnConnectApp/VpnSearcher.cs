using NonInvasiveKeyboardHookLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Net.NetworkInformation;
using VPNConnect.VpnClientHandling;
using VPNConnect.Net;
using VpnConnect.Configuration;
using GeoIpDb.Repo;
using System.Net.Mail;
using GeoIpDb.Entities;

namespace VPNConnect
{
    internal class VpnSearcher
    {
        public bool IsStarted { get; private set; }

        private readonly IVpnUiHandler vpnUiHandler;
        private readonly VpnSearchSettings settings;
        private string currentIp;
        ExternalIpServiceProvider externalIpServiceProvider;
        private GeoIpCityRepository geoIpCityRepository;
        private GeoIpAsnRepository geoIpAsnRepository;
        private KnownIpPoolRepository knownIpRepository;
        private List<GeoIpCountry> blacklistCountries;

        public VpnSearcher(IVpnUiHandler vpnUiHandler , VpnSearchSettings settings)
        {
            this.vpnUiHandler = vpnUiHandler;
            this.settings = settings;
            externalIpServiceProvider = new ExternalIpServiceProvider(settings.ExternalIpServiceLink);
            geoIpCityRepository = new GeoIpCityRepository(settings.GeoIpDbSettings.ConnectionString);
            geoIpAsnRepository = new GeoIpAsnRepository(settings.GeoIpDbSettings.ConnectionString);
            knownIpRepository = new KnownIpPoolRepository(settings.GeoIpDbSettings.ConnectionString);
            blacklistCountries = new GeoIpCountryRepository(settings.GeoIpDbSettings.ConnectionString)
                .GetBlacklistedList(settings.TargetApplicationSettings.ApplicationId);
        }

        public string GetMyPublicIp()
        {
            var ip = externalIpServiceProvider.GetMyIp();
            if (!ip.IsSuccess)
            {
                Log.Error($"Can't get current connection info, check your Internet connection");
                return null;
            }
            Log.Information($"Public IP is {ip.IpAddress}");
            return ip.IpAddress;
        }

        public void StopSearch()
        {
            if (IsStarted)
            {
                Log.Information($"Stopping search");
                IsStarted = false;
            }
        }

        public void StartSearch(string initialIp)
        {
            {
                if (IsStarted) return;
                IsStarted = true;
                Log.Information($"{settings.ConsoleSettings.StartHotKey} pressed");
                Log.Information("VPN searching is started");

                NetQualityAnalyzer netQualityAnalyzer = new(settings.NetAnanlyzeSettings.PingTarget,
                    settings.NetAnanlyzeSettings.PingHops, blacklistCountries.Select(c=>c.CountryId).ToList());

                while (IsStarted)
                {
                    try
                    {

                        Log.Information("Emulate mouse left click on VPN client CONNECT button");

                        vpnUiHandler.PressConnect();

                        Log.Information($"Waiting for VPN connection {settings.VpnUiHandlingSettings.ConnectTimeoutSec} sec");

                        var connectionWaitingResult = externalIpServiceProvider.WaitForMyIpChanging(initialIp, settings.VpnUiHandlingSettings.ConnectTimeoutSec);

                        if (connectionWaitingResult.IsTimeout)
                        {
                            Log.Information($"Connection timeout, let's retry");

                            Disconect();
                        }
                        else if (connectionWaitingResult.IsSuccess)
                        {
                            currentIp = connectionWaitingResult.IpAddress;
                            Log.Information($"Connected. My IP: {currentIp}");

                            var geoipCityInfo = geoIpCityRepository.GetByIpAddress(currentIp);
                            var geoipAsnInfo = geoIpAsnRepository.GetByIpAddress(currentIp);

                            if (geoipCityInfo != null)
                            {
                                Log.Information($"The VPN geoip city info: country code: {geoipCityInfo.CountryID}; city: {geoipCityInfo.CityName}");
                            }
                            else
                            {
                                Log.Information($"The VPN geoip info is not found");
                            }

                            if (geoipAsnInfo != null)
                            {
                                Log.Information($"The VPN's ASN Name: {geoipAsnInfo.Asn.Name}");
                            }
                            else
                            {
                                Log.Information($"The VPN's ASN info is not found");
                            }

                            bool isCountryBlacklisted = netQualityAnalyzer.IsCountryBlacklisted(geoipCityInfo != null ? geoipCityInfo.CountryID : "");

                            if (isCountryBlacklisted)
                            {
                                Log.Information($"The VPN country is in your blacklist");
                                Disconect();
                            }
                            else
                            {
                                var knownIp = knownIpRepository.GetByIpAddress(currentIp);
                                if (knownIp != null && (knownIp.ApplicationId==null || knownIp.ApplicationId==settings.TargetApplicationSettings.ApplicationId))
                                {
                                    if (knownIp.IsBlacklisted)
                                    {
                                        Log.Information($"The VPN IP is in your blacklist");
                                        Disconect();
                                    }
                                    else NetAnalyze(netQualityAnalyzer);
                                }
                                else NetAnalyze(netQualityAnalyzer);

                            }

                        }
                        else
                        {
                            Log.Error("Can't connect, check your VPN client");
                            IsStarted = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error on VPN Search: {ex}");
                        IsStarted = false;
                    }
                }

                Log.Information("VPN searching has stopped");

            }
        }

        private void NetAnalyze(NetQualityAnalyzer netQualityAnalyzer)
        {
            Log.Information($"Started network quality analyzing with {settings.NetAnanlyzeSettings.PingTarget} as a target");
            var netQuality = netQualityAnalyzer.Analyze(settings.NetAnanlyzeSettings.TolerablePacketLoss);
            if (netQuality.IsValid)
            {
                Log.Information($"Packets lost: {netQuality.LostPackets}; avg latency: {netQuality.AvgLatency}");
                if (netQuality.LostPackets < settings.NetAnanlyzeSettings.TolerablePacketLoss
                    && netQuality.AvgLatency < settings.NetAnanlyzeSettings.TolerableLatencySec)
                {
                    Log.Information("The VPN is good enough, let's stop here");
                    IsStarted = false;
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

        private void Disconect()
        {
            Log.Information("Disconnecting");
            Log.Information("Emulating mouse left click on VPN client DISCONNECT button");
            vpnUiHandler.PressDisconnect();
            Log.Information($"Waiting for disconnect {settings.VpnUiHandlingSettings.ConnectTimeoutSec} sec");

            var disconnectWaitingResult = externalIpServiceProvider.WaitForMyIpChanging(currentIp, settings.VpnUiHandlingSettings.ConnectTimeoutSec);

            if (!disconnectWaitingResult.IsSuccess||disconnectWaitingResult.IsTimeout)
            {
                Log.Error("Can't disconnect, check your VPN client");
                IsStarted = false;
            }
            else
            {
                Log.Information("Disconnected");
                Log.Information($"Waiting {settings.VpnUiHandlingSettings.DelayBetweenConnectionAttemptsSec} sec before the next attempt");
                Thread.Sleep(TimeSpan.FromSeconds(settings.VpnUiHandlingSettings.DelayBetweenConnectionAttemptsSec));
            }

        }

    }
}
