﻿using AluminiumTech.DevKit.PlatformKit;

using System;
using System.Diagnostics;

namespace PlatformKit.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Platform platform = new Platform();
            ProcessManager processManager = new ProcessManager();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Console.WriteLine("Desc: " +  System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("TFM: " + System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier);
            Console.WriteLine("OsVersion: " + Environment.OSVersion);
            Console.WriteLine("Framework: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
            
    //        processManager.SuspendProcess("Spotify");

      //      Console.WriteLine("Start suspending...");

        //    PerformanceCounter

            while (stopwatch.ElapsedMilliseconds < (1 * 1000))
            {

            }
            processManager.ResumeProcess("Spotify");

            Console.WriteLine("Start resuming...");

            Console.ReadLine();
        }
    }
}