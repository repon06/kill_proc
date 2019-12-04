/*
 * User: BystrovDM
 */

using System;
using System.Linq;
using System.Diagnostics;
using NLog;

namespace killer_service
{
    public static class Processes
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void show_all_process()
        {
            var allProcess = from proc in Process.GetProcesses(".")
                orderby proc.Id
                select proc;

            foreach (var proc in allProcess)
            {
                try
                {
                    Console.WriteLine(@"PID:{0}; Name:{1}; StartTime:{2}", proc.Id, proc.ProcessName, proc.StartTime);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(@"ERROR: {0}: {1}", e.Message, proc.ProcessName);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public static void show_process(string procName)
        {
            var allProcess = from proc in Process.GetProcesses(".")
                orderby proc.Id
                where proc.ProcessName.Equals(procName)
                select proc;

            foreach (var proc in allProcess)
            {
                try
                {
                    Console.WriteLine(@"PID:{0}; Name:{1}; StartTime:{2}", proc.Id, proc.ProcessName, proc.StartTime);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(@"ERROR: {0}: {1}", e.Message, proc.ProcessName);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }


        public static void kill_old_process(string procName, int liveDuration)
        {
            var allProcess = from proc in Process.GetProcesses(".")
                orderby proc.Id
                where proc.ProcessName.Equals(procName) && proc.StartTime.AddMinutes(liveDuration) < DateTime.Now
                select proc;

            foreach (var proc in allProcess)
            {
                //if (DateTime.Now > proc.StartTime.AddMinutes(live_duration)) 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(@"Kill process: pid:{0}; Name: {1}; StartTime: {2}", proc.Id, proc.ProcessName,
                        proc.StartTime);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    proc.Kill();
                    Log.Info(@"Kill process: pid:{0}; Name: {1}; StartTime: {2}.", proc.Id, proc.ProcessName,
                        proc.StartTime);
                }
            }
        }
    }
}