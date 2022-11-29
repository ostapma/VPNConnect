using System;
using System.Collections.Generic;
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

            view.OnSelected += (name) =>
            {
                OnSelected(vpnServiceFactory.Get(name));
            };
        }

        public void Select()
        {
            view.AskSelect(vpnServiceFactory.GetList().Select(s => s.Name).ToList());
        }

        public void ShowSelected(VpnService service)
        {
            view.ShowSelected(service.Name);
        }

        public event Action<VpnService?> OnSelected;
    }
}
