﻿/* MIT License

Copyright (c) 2019-2021 AluminiumTech

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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    /// A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {
        protected Platform _platform;

        public ProcessManager()
        {
            _platform = new Platform();
        }

        /// <summary>
        /// Run a Process with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcess(string processName, string arguments = "")
        {
            RunActionOn(() => RunProcessWindows(processName, arguments), () => RunProcessMac(processName, arguments),
                () => RunProcessLinux(processName, arguments));
        }

        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        /// <param name="pws"></param>
        public void RunProcessWindows(string processName, string arguments = "",
            ProcessWindowStyle pws = ProcessWindowStyle.Normal)
        {
            try
            {
                Process process = new Process();

                if (processName.Contains(".exe") || processName.EndsWith(".exe"))
                {
                    ;
                    process.StartInfo.FileName = processName;
                }
                else
                {
                    process.StartInfo.FileName = processName + ".exe";
                }

                process.StartInfo.Arguments = arguments;
                process.StartInfo.WindowStyle = pws;
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Run a Process on macOS
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcessMac(string processName, string arguments = "")
        {
            var procStartInfo = new ProcessStartInfo()
            {
                FileName = processName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = arguments
            };

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }

        // This won't be implemented for V2. This will be implemented in V2.1 or later
        /*
        public void RunConsoleCommand(string arguments)
        {
            var plat = new Platform().ToEnum();

            string programName = "";
            
            if (plat.Equals(OperatingSystemFamily.Windows))
            {
                programName = "cmd";
            }
            else if (plat.Equals(OperatingSystemFamily.macOS))
            {
           //     programName = "open -b com.apple.terminal " + arguments;
            }
            else if (plat.Equals(OperatingSystemFamily.Linux))
            {
                //programName
            }

            RunProcess(programName, arguments);
        }
         */

        /// <summary>
        /// TODO: Test, Fix, and Revamp this method as required to ensure it is working for V2.1
        /// THIS WILL BE DISABLED FOR V2.
        /// 
        /// WARNING: Does not work on Windows or macOS.
        /// </summary>
        /// <param name="command"></param>
        /* internal void RunCommandLinux(string command)
        {
            try
            {
                if (GetPlatformOperatingSystemFamily() != (OperatingSystemFamily.Linux))
                {
                    throw new PlatformNotSupportedException();
                }

                //https://askubuntu.com/questions/506985/c-opening-the-terminal-process-and-pass-commands
                string processName = "/usr/bin/bash";
                string processArguments = "-c \" " + command + " \"";

                RunProcessLinux(processName, processArguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        */
        
        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="processArguments"></param>
        public void RunProcessLinux(string processName, string processArguments = "")
        {
            try
            {
                var procStartInfo = new ProcessStartInfo()
                {
                    FileName = processName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    Arguments = processArguments
                };

                Process process = new Process { StartInfo = procStartInfo };
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// Run different actions or methods depending on the operating system.
        /// </summary>
        /// <param name="windowsMethod"></param>
        /// <param name="macMethod"></param>
        /// <param name="linuxMethod"></param>
        /// <param name="freeBsdMethod"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool RunActionOn(System.Action windowsMethod = null, System.Action macMethod = null,
            System.Action linuxMethod = null, System.Action freeBsdMethod = null)
        {
            try
            {
                if (_platform.IsWindows() && windowsMethod != null)
                {
                    windowsMethod.Invoke();
                    return true;
                }

                if (_platform.IsLinux() && linuxMethod != null)
                {
                    linuxMethod.Invoke();
                    return true;
                }
                if (_platform.IsMac() && macMethod != null)
                {
                    macMethod.Invoke();
                    return true;
                }
                
#if NETCOREAPP3_0_OR_GREATER
                if (_platform.IsFreeBSD() && freeBsdMethod != null)
                {
                    freeBsdMethod.Invoke();
                    return true;
                }
#endif
                if (_platform.IsMac() && macMethod == null ||
                    _platform.IsLinux() && linuxMethod == null ||
                    _platform.IsWindows() && windowsMethod == null)
                {
                    throw new ArgumentNullException();
                }

                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// Check to see if a process is running or not.
        /// </summary>
        public bool IsProcessRunning(string processName)
        {
            foreach (Process proc in Process.GetProcesses())
            {
              var procName =  proc.ProcessName.Replace("System.Diagnostics.Process (", String.Empty);

                //Console.WriteLine(proc.ProcessName);

                processName = processName.Replace(".exe", String.Empty);
                
                if (procName.ToLower().Equals(processName.ToLower()) ||
                    procName.ToLower().Contains(processName.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a String to a Process
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public Process ConvertStringToProcess(string processName)
        {
            try
            {
                processName = processName.Replace(".exe", "");

                if (IsProcessRunning(processName) ||
                    IsProcessRunning(processName.ToLower()) ||
                    IsProcessRunning(processName.ToUpper())
                )
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process p in processes)
                    {
                        if (p.ProcessName.ToLower().Equals(processName.ToLower()))
                        {
                            return p;
                        }
                    }
                }

                return null;
                //  throw new Exception();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// Get the list of processes as a String Array
        /// </summary>
        /// <returns></returns>
        public string[] GetProcessesToStringArray()
        {
            var strList = new List<string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                strList.Add(process.ProcessName);
            }

            strList.TrimExcess();
            return strList.ToArray();
        }


        /// <summary>
        /// Open a URL in the default browser.
        ///
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool OpenUrlInBrowser(string url)
        {
            try
            {
                if (_platform.IsWindows())
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}")
                        { CreateNoWindow = true });
                    return true;
                }
                else if (_platform.IsLinux())
                {
                    Process.Start("xdg-open", url);
                    return true;
                }
                else if (_platform.IsMac())
                {
                    Process.Start("open", url);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Suspends a process using native or imported method calls.
        /// </summary>
        /// <param name="processName"></param>
        /// <exception cref="PlatformNotSupportedException">This is currently only implemented on Windows and will throw an exception if run on Linux or macOS.</exception>
        public void SuspendProcess(string processName)
        {
            if (_platform.IsWindows())
            {
                PlatformSpecifics.WindowsProcessSpecifics.Suspend(ConvertStringToProcess(processName));
            }
            else if (_platform.IsLinux())
            {
                throw new PlatformNotSupportedException();
            }
            else if (_platform.IsMac())
            {
                throw new PlatformNotSupportedException();
            }
#if NETCOREAPP3_0_OR_GREATER
            else if (_platform.IsFreeBSD())
            {
                throw new PlatformNotSupportedException();
            }
#endif
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Resumes a process using native or imported method calls.
        /// WARNING: This is only implemented on Windows and will throw an exception if run on Linux or macOS.
        /// </summary>
        /// <param name="processName"></param>
        public void ResumeProcess(string processName)
        {
            if (_platform.IsWindows())
            {
                if (IsProcessRunning(processName))
                {
                    PlatformSpecifics.WindowsProcessSpecifics.Resume(ConvertStringToProcess(processName));
                }
            }
            else if (_platform.IsLinux())
            {
                throw new PlatformNotSupportedException();
            }
            else if (_platform.IsMac())
            {
                throw new PlatformNotSupportedException();
            }
#if NETCOREAPP3_0_OR_GREATER
            else if (_platform.IsFreeBSD())
            {
                throw new PlatformNotSupportedException();
            }
#endif
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Determines whether a process (or the current process if unspecified) is running as an administrator.
        /// Currently only supports Windows. Running on macOS or Linux will return a PlatformNotSupportedException.
        /// WORK IN PROGRESS. 
        /// Fix for future 2.x release
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Occurs when running on macOS or Linux as these are not currently supported.</exception>
        public bool IsRunningAsAdministrator(Process process = null)
        {
            try
            {
                if (process == null)
                {
                    process = Process.GetCurrentProcess();
                }
                
                if (_platform.IsWindows())
                {

                    if (process.StartInfo.Verb.Contains("runas"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                /*       else if (platform.ToOperatingSystemFamily().Equals(OperatingSystemFamily.macOS))
                     {
                         return (Mono.Unix.Native.Syscall.geteuid() == 0);
                     }
                     else if (platform.ToOperatingSystemFamily().Equals(OperatingSystemFamily.Linux))
                     {
                         return (Mono.Unix.Native.Syscall.geteuid() == 0);
                     }
    
              */
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
    }
}