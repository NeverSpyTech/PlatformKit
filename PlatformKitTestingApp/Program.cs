﻿/* MIT License

Copyright (c) 2018-2021 AluminiumTech

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

// See https://aka.ms/new-console-template for more information

using System;
using System.Runtime.InteropServices;
using System.Text;

using PlatformKit.Identification;
using PlatformKit.Identification.Enums;

using PlatformKit;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

var platformManager = new PlatformIdentification();
            var osAnalyzer = new OSAnalyzer();
            var processManager = new ProcessManager();
var runtimeIdentification = new RuntimeIdentification();

var windowsAnalyzer = new WindowsAnalyzer();
var linuxAnalyzer = new LinuxAnalyzer();
var macAnalyzer = new MacOSAnalyzer();

            Console.WriteLine(".NET Detected RuntimeID: " + RuntimeInformation.RuntimeIdentifier);

            var title = $"{platformManager.GetAppName()} v{platformManager.GetAppVersion()}";
            Console.Title = title;
            //    Console.WriteLine(title) ;
          
            Console.WriteLine("Programmatically Generated RuntimeID (AnyGeneric): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric));
            Console.WriteLine("Programmatically Generated RuntimeID (Generic): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic));
            Console.WriteLine("Programmatically Generated RuntimeID (Specific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));

            foreach (var candidate in runtimeIdentification.GetPossibleRuntimeIdentifierCandidates())
            {
                Console.WriteLine("Possible RID: " + candidate);
            }
          
            //processManager.OpenUrlInBrowser("duckduckgo.com");
            
            if (OSAnalyzer.IsWindows())
            {
                Console.WriteLine("Windows Version Enum: " + windowsAnalyzer.GetWindowsVersionToEnum());

             //   var res = processManager.RunPowerShellCommand("ping www.duckduckgo.com");
                
             //  Console.WriteLine(res);
             
               // var caption = windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem");
               // var name = windowsAnalyzer.GetWMIValue("Name", "Win32_OperatingSystem");
                
                //Console.WriteLine("Caption: " + caption);
                //Console.WriteLine("Name: " + name);
            
           //     Console.WriteLine("OS :" + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
                
                //Console.WriteLine("Caption: " + windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem"));

               // Console.WriteLine(processManager.RunPowerShellCommand("systeminfo"));
               Console.WriteLine("WindowsEdition: " + windowsAnalyzer.DetectWindowsEdition().ToString());
               
               Console.WriteLine("WinOS " + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
               
              /* var desc = processManager.RunPowerShellCommand("systeminfo");

               var arr = desc.Split(Environment.NewLine);
               
                   //stringBuilder.ToString().Split(" ");

               for (int index = 0; index < arr.Length; index++)
               {
                    Console.WriteLine(index + " " + arr[index]);
               }
               */

              var sysinfo = windowsAnalyzer.GetWindowsSystemInformation();
              sysinfo.ToConsoleWriteLine();
            }

            if (OSAnalyzer.IsMac())
            {
                Console.WriteLine("Is this AppleSilicon?: " + macAnalyzer.IsAppleSiliconMac());
                Console.WriteLine("Mac Processor Type: " + macAnalyzer.GetMacProcessorType());

                Console.WriteLine("macOS Version Enum: " +
                                  macAnalyzer.GetMacOsVersionToEnum());
                
                Console.WriteLine("macOS Version Detected: " + osAnalyzer.DetectOSVersion().ToString());
                
                Console.WriteLine("Darwin Version: " + macAnalyzer.DetectDarwinVersion());
                Console.WriteLine("Xnu Version: " + macAnalyzer.DetectXnuVersion());
            }
 
            // Console.WriteLine("OsVersion: " + versionAnalyzer.DetectOSVersion());
    
            if (OSAnalyzer.IsLinux())
            {  
                Console.WriteLine("Linux Distro Name: " + linuxAnalyzer.GetLinuxDistributionInformation().Name);
                Console.WriteLine("Linux Distro Version: " + linuxAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + linuxAnalyzer.DetectLinuxKernelVersion());
            }

Console.WriteLine("PlatformKit Version: " + PlatformIdentification.GetPlatformKitVersion().ToString());

            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);