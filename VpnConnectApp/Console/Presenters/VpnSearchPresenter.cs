using NonInvasiveKeyboardHookLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Configuration;
using VpnConnect.Console.Views;
using VpnConnect.VpnServices;
using VPNConnect;

namespace VpnConnect.Console.Presenters
{
    internal class VpnSearchPresenter
    {
        private readonly VpnService service;
        private readonly VpnSearchSettings settings;
        private readonly VpnSearcher searcher;
        KeyboardHookManager keyboardHookManager = new();
        private VpnSearchView view = new VpnSearchView();


        public VpnSearchPresenter(VpnService service, VpnSearchSettings settings, VpnSearcher searcher, Action onStopped)
        {
            this.service = service;
            this.settings = settings;
            this.searcher = searcher;
            searcher.OnSearchStopped += Searcher_OnSearchStopped;
            searcher.OnSearchStopped += onStopped;
        }

        private void Searcher_OnSearchStopped()
        {
            UnsubscribeKeys();
            view.ShowSearchStopped();
        }

        void SubscribeKeys()
        {

            keyboardHookManager.Start();

            keyboardHookManager.RegisterHotkey(GetVcode(settings.ConsoleSettings.StopHotKey), () =>
                StopSearch());

            keyboardHookManager.RegisterHotkey(GetVcode(settings.ConsoleSettings.StartHotKey), () =>
                Search());
        }


        void UnsubscribeKeys()
        {
            keyboardHookManager.UnregisterHotkey(GetVcode(settings.ConsoleSettings.StopHotKey));
            keyboardHookManager.UnregisterHotkey(GetVcode(settings.ConsoleSettings.StartHotKey));
            keyboardHookManager.Stop();
        }

        private int GetVcode(string code)
        {
            return (int)Enum.Parse(typeof(Keys), code);
        }

        public void Search()
        {
            string myIp = searcher.GetMyPublicIp();
            if (myIp == null)
            {
                view.ShowGetMyIpError();
            }
            else
            {
                view.ShowMyIp(myIp);
                searcher.StartSearch(myIp);
            }
        }

        void StopSearch()
        {
            
            view.ShowSearchStop(settings.ConsoleSettings.StopHotKey);
            
            searcher.StopSearch();
        }

        internal void Start()
        {
            SubscribeKeys();
            view.ShowStartPrompt(service.Name,settings.ConsoleSettings.StartHotKey, settings.ConsoleSettings.StopHotKey);
        }
    }
}
