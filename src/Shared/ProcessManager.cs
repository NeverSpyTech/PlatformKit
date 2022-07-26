/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;


using PlatformKit.Internal.Licensing;

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace PlatformKit
{
    /// <summary>
    ///     A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {
        // ReSharper disable once InconsistentNaming
        protected readonly OSAnalyzer _osAnalyzer;

        public ProcessManager()
        {
            _osAnalyzer = new OSAnalyzer();

            PlatformKitConstants.CheckLicenseState();
        }

        /// <summary>
        ///  Run a Process with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">(Optional) Arguments to be passed to the executable.</param>
        /// <param name="processStartInfo">(Optional) Process Start Information to be passed to the executable. This may have the unintended effect of overriding other optional or non-optional arguments.</param>
        public string RunProcess(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            if (OSAnalyzer.IsWindows()) return RunProcessWindows(executableLocation, executableName, arguments, processStartInfo);
            if (OSAnalyzer.IsLinux()) return RunProcessLinux(executableLocation, executableName, arguments, processStartInfo);
            if (OSAnalyzer.IsMac()) return RunProcessMac(executableLocation, executableName, arguments, processStartInfo);

#if NETCOREAPP3_0_OR_GREATER
            if (OSAnalyzer.IsFreeBSD()) throw new NotImplementedException();
#endif
            
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        /// <param name="windowStyle">Whether to open the window in full screen mode, normal size, minimized, or a different window mode.</param>
        /// <exception cref="Exception"></exception>
        public string RunProcessWindows(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null,
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            try
            {
                Process process;
            
                if (processStartInfo != null)
                {
                    processStartInfo.WorkingDirectory = executableLocation;
                    processStartInfo.FileName = executableName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.RedirectStandardOutput = true;
                    process = new Process { StartInfo = processStartInfo};
                }
                else
                {
                    process = new Process();
                    
                    process.StartInfo.FileName = executableName;

                    if (!executableName.EndsWith(".exe") && insertExeInExecutableNameIfMissing)
                    {
                        process.StartInfo.FileName += ".exe";
                    }

                    process.StartInfo.WorkingDirectory = executableLocation;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.WindowStyle = windowStyle;
                }

                if (runAsAdministrator)
                {
                    process.StartInfo.Verb = "runas";
                }

                Task task = new Task(() => process.Start());
                task.Start();

                task.Wait();
                var end = process.StandardOutput.ReadToEnd();

                if (end == null)
                {
                    throw new NullReferenceException();
                }

                if (end.ToLower()
                    .Contains(" is not recognized as the name of a cmdlet, function, script file, or operable program"))
                {
                    throw new Exception(end);
                }
                else if (end.ToLower()
                         .Contains(
                             "is not recognized as an internal or external command, operable program or batch file."))
                {
                    throw new Exception(end);
                }

                return end;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        ///  Run a Process on macOS
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        public string RunProcessMac(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            var procStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = executableLocation,
                FileName = executableName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = arguments
            };

            Process process;
            
            if (processStartInfo == null)
            {
                process = new Process { StartInfo = procStartInfo };
            }
            else
            {
                procStartInfo = processStartInfo;
                procStartInfo.WorkingDirectory = executableLocation;
                procStartInfo.FileName = executableName;
                procStartInfo.Arguments = arguments;
                procStartInfo.RedirectStandardOutput = true;
                process = new Process { StartInfo = processStartInfo};
            }
            
            process.Start();

            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string RunProcessLinux(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            try
            {
                ProcessStartInfo procStartInfo;
                
                if (processStartInfo == null)
                {
                     procStartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = executableLocation,
                        FileName = executableName,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        Arguments = arguments
                    };
                }
                else
                {
                    procStartInfo = processStartInfo;
                    procStartInfo.WorkingDirectory = executableLocation;
                    procStartInfo.FileName = executableName;
                    procStartInfo.Arguments = arguments;
                    procStartInfo.RedirectStandardOutput = true;
                }
                
                var process = new Process { StartInfo = procStartInfo };
                process.Start();

                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Runs commands in the Windows Cmd Command Prompt.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunCmdCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            return RunProcessWindows(Environment.SystemDirectory, "cmd", command, processStartInfo);
        }

        /// <summary>
        /// Runs commands in Windows Powershell.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunPowerShellCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            if (OSAnalyzer.IsWindows())
            {
                var location = Environment.SystemDirectory + Path.DirectorySeparatorChar 
                                                           //+ "System32" +
                               + Path.DirectorySeparatorChar + "WindowsPowerShell" +
                               Path.DirectorySeparatorChar + "v1.0";
                return RunProcessWindows(location, "powershell", command, processStartInfo);
   
            }
                
            throw new PlatformNotSupportedException();
        }

        
        /// <summary>
        /// Run a command on macOS located in the /usr/bin/ folder.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public string RunMacCommand(string command)
        {
            if (OSAnalyzer.IsMac())
            {
                var location = "/usr/bin/";
                
                string[] array = command.Split(' ');

                if (array.Length > 1)
                {
                    string args = "";

                    foreach (string arg in array)
                    {
                        args += arg + " ";
                    }

                    args = args.Replace(array[0], String.Empty);

                   return RunProcessMac(location, array[0], args);
                }
                else
                {
                   return RunProcessMac(location, command);
                }
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Run a command or program as if inside a terminal on Linux.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string RunLinuxCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            try
            {
                if (OSAnalyzer.IsLinux())
                {
                    var location = "/usr/bin/";

                    var args = command.Split(' ');
                    command = args[0];
                    var processArguments = "";

                    if (args.Length > 0)
                    {
                        for (int index = 1; index < args.Length; index++)
                        {
                            processArguments += args[index].Replace(command, String.Empty);
                        }
                    }

                    if (!Directory.Exists(location))
                    {
                        throw new DirectoryNotFoundException("Could not find directory '" + nameof(location) + "' with value: " + location);
                    }
                    
                    return RunProcessLinux(location, command, processArguments, processStartInfo);
                }

                throw new PlatformNotSupportedException();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// Open a URL in the default browser.
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url">The URL to be opened.</param>
        /// <param name="allowNonSecureHttp">Whether to allow non HTTPS links to be opened.</param>
        /// <returns></returns>
        public bool OpenUrlInBrowser(string url, bool allowNonSecureHttp = false)
        {
            try
            {
                if ((!url.StartsWith("https://") || !url.StartsWith("www.")) && (!url.StartsWith("file://")))
                {
                    if (allowNonSecureHttp)
                    {
                        url = "http://" + url;
                    }
                    else
                    {
                        url = "https://" + url;
                    }
                }
                else if (url.StartsWith("http://") && !allowNonSecureHttp)
                {
                    url = url.Replace("http://", "https://");
                }

                if (OSAnalyzer.IsWindows())
                {
                    RunCmdCommand($"/c start {url.Replace("&", "^&")}", new ProcessStartInfo { CreateNoWindow = true});
                    return true;
                }
                if (OSAnalyzer.IsLinux())
                {
                    RunLinuxCommand("xdg-open " + url);
                    return true;
                }
                if (OSAnalyzer.IsMac())
                {
                    var task = new Task(() =>
                        Process.Start("open", url));
                    task.Start();
                    return true;
                }
#if  NETCOREAPP3_1_OR_GREATER
                if (OSAnalyzer.IsFreeBSD())
                {
                    throw new NotImplementedException();
                }          
#endif

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}