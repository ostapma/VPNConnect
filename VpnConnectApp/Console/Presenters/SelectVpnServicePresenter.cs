using Autofac.Core;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Views;
using VpnConnect.VpnServices;

namespace VpnConnect.Console.Presenters
{
    internal class SelectVpnServicePresenter
    {
        SelectVpnServiceView view;

        public VpnServiceFactory vpnServiceFactory;

        public SelectVpnServicePresenter(VpnServiceFactory vpnServiceFactory)
        {
            view= new SelectVpnServiceView();
            this.vpnServiceFactory = vpnServiceFactory;

        }

        public void ShowSelector() {
            view.AskSelect(vpnServiceFactory.GetList().Select(s => s.Name).ToList(), SelectVpn);
        }

        public void SelectVpn(string name)
        {
            var selectedService = vpnServiceFactory.Get(name);
            if (selectedService != null)
            {
                OnSelected.Invoke(selectedService);
                view.ShowSelected(name);
            }
            else throw new ArgumentException($"Invalid value {name}");
        }


        public event Action<VpnService> OnSelected;
    }
}
