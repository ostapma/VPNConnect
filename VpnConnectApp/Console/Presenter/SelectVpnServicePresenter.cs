using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.View;
using VpnConnect.VpnServices;

namespace VpnConnect.Console.Presenter
{
    internal class SelectVpnServicePresenter
    {
        SelectVpnServiceView view;

        public VpnServiceFactory vpnServiceFactory;

        public SelectVpnServicePresenter(VpnServiceFactory vpnServiceFactory)
        {
            view= new SelectVpnServiceView();
            this.vpnServiceFactory = vpnServiceFactory;
            view.OnSelected = (name) =>
            {
                OnSelected(vpnServiceFactory.Get(name));
            };
        }

        public void Select()
        {
            view.ShowSelect(vpnServiceFactory.GetList().Select(s => s.Name).ToList());
        }

        public Action<VpnService?> OnSelected;
    }
}
