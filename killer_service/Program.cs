/*
 * User: BystrovDM
 */

using System;
using System.Timers;
using NLog;

namespace killer_service
{
    internal static class Program
    {
        private static string _processName;
        private static int _live;
        private static int _interval;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            if (args != null && args.Length == 3)
            {
                var flag = true;
                const string stopFlag = "q";

                Console.WriteLine("Enter '{0}' to exit.", stopFlag);

                _processName = args[0];
                _live = Convert.ToInt16(args[1]);
                _interval = Convert.ToInt16(args[2]);

                Log.Info("Run program at {0}. Search process: '{1}'. Max Live: {2} min. Interval check: {3} min.",
                    DateTime.Now, _processName, _live, _interval);

                var timer = new System.Timers.Timer(60 * 1000 * _interval);
                timer.Elapsed += OnTimeout;
                timer.AutoReset = true;
                timer.Enabled = true;

                do
                {
                    var key = Console.ReadLine();
                    if (key != null) flag &= !key.Equals(stopFlag);
                } while (flag);
            }

            Console.WriteLine("Program is shutdown.");
            Log.Info("Program is shutdown at {0}.", DateTime.Now);
        }

        private static void OnTimeout(Object source, ElapsedEventArgs e)
        {
            var curTime = DateTime.Now;
            Console.WriteLine("Current time: {0}", curTime);

            Processes.show_process(_processName);
            Processes.kill_old_process(_processName, _live);
        }
    }
}