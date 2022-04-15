using Microsoft.Owin.Hosting;
using SelfHostWeb.Server;
using System;
using System.Configuration;

namespace SelfHostWeb.WebApi
{
    public class WebServer
    {
        static IDisposable server = null;
        public static event EventHandles.MessageEventHandle MessageEvent = null;

        public static void Start()
        {
            var opt = new StartOptions();
            opt.Port = int.Parse(ConfigurationManager.AppSettings.Get("Port"));

            string baseAddress = string.Format("http://{0}:{1}/", "*",ConfigurationManager.AppSettings.Get("Port"));
            server = WebApp.Start<Startup>(url: baseAddress);
            
            MessageEvent?.Invoke(String.Format("host 已启动：{0}", baseAddress));
        }
    }
}
