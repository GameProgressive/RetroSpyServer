﻿using Serilog.Events;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;

namespace ServerBrowser.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                new SBServerManager(UniSpyServerName.SB).Start();
                Console.Title = "RetroSpy Server " + UniSpyServerManagerBase.RetroSpyVersion;
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }

            Console.WriteLine("Press < Q > to exit. ");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
