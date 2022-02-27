using SelfHostWeb.WebApi;
using System;

namespace SelfHostWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
            Console.ReadLine();
        }

        public static void StartServer()
        {
            WebServer.MessageEvent += WebServer_MessageEvent;
            WebServer.Start();
        }

        private static void WebServer_MessageEvent(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
