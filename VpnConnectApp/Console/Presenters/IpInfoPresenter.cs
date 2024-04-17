using GeoIpDb.Repo;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Views;
using VPNConnect.Net;

namespace VpnConnect.Console.Presenters
{
    internal class IpInfoPresenter
    {
        private readonly ExternalIpServiceProvider externalIpServiceProvider;
        private readonly GeoIpCityRepository geoIpCityRepository;
        private readonly GeoIpAsnRepository geoIpAsnRepository;
        private KnownIpPoolRepository knownIpRepository;
        IpInfoView view;
        public IpInfoPresenter(ExternalIpServiceProvider externalIpServiceProvider,
            GeoIpCityRepository geoIpCityRepository, GeoIpAsnRepository geoIpAsnRepository,
            KnownIpPoolRepository knownIpRepository) { 
            view= new IpInfoView();
            this.externalIpServiceProvider = externalIpServiceProvider;
            this.geoIpCityRepository = geoIpCityRepository;
            this.geoIpAsnRepository = geoIpAsnRepository;
            this.knownIpRepository = knownIpRepository;
        }



        public void IpInfo(string? ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                var result = externalIpServiceProvider.GetMyIp();
                if (result.IsSuccess)
                {
                    view.ShowYorCurrentIp(result.IpAddress);
                    ShowIpInfo(result.IpAddress);
                }
                else
                {
                    view.ShowYorCurrentIpError();
                }
            }
            else
            {
                try
                {
                    IPAddress.Parse(ip);
                    ShowIpInfo(ip);
                }
                catch (FormatException e)
                {
                    view.ShowIpFormatError(ip);
                }
            }
        }

        private void ShowIpInfo(string ip)
        {
            var cityInfo =  geoIpCityRepository.GetByIpAddress(ip);
            if (cityInfo != null)
                view.ShowLocationIpInfo(ip, cityInfo.CountryID, cityInfo.RegionName);
            else view.ShowLocationInfoNotFound(ip);
            var asnInfo = geoIpAsnRepository.GetByIpAddress(ip);
            if (asnInfo != null)
                view.ShowOwnerIpInfo(ip, asnInfo.Asn.Name);
            else view.ShowOwnerInfoNotFound(ip);
            var knownIpInfo = knownIpRepository.GetByIpAddress(ip);
            if (knownIpInfo != null)
                view.ShowKnownIpInfo(knownIpInfo.DateAdded, knownIpInfo.Comments, knownIpInfo.IsBlacklisted, knownIpInfo.IsGood);
        }

        
    }
}
