/* MIT License

Copyright (c) 2018-2022 AluminiumTech

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
using AluminiumTech.DevKit.PlatformKit.Analyzers;

namespace AluminiumTech.DevKit.PlatformKit.Software.Windows;

public class WindowsSoftwareDetection : IWindowsSoftwareDetection
{
    protected OSAnalyzer _osAnalyzer;
    protected ProcessManager _processManager;
    
    public WindowsSoftwareDetection()
    {
        _osAnalyzer = new OSAnalyzer();
        _processManager = new ProcessManager();
    }
    
        /// <summary>
        ///  Gets the value of a registry key in the Windows registry.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="failMessage"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
        public string GetWindowsRegistryValue(string query, string value, string failMessage){
            if (_osAnalyzer.IsWindows())
            {
                try{
                    var result = _processManager.RunCmdCommand("/c REG QUERY " + query + " /v " + value);
                    
                    if (result != null)
                    {
                        result = result.Replace(value, String.Empty)
                            .Replace("REG_SZ", String.Empty)
                            .Replace(" ", String.Empty);
                        return result;
                    }
                }
                catch{
                    return failMessage;
                }
            }

            throw new PlatformNotSupportedException();
        }

        public string GetWMIValue(string query, string wmiClass, string failMessage)
        {
            return GetWindowsManagementInstrumentationValue(query, wmiClass, failMessage);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="wmiClass"></param>
        /// <param name="failMessage"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
        // ReSharper disable once InconsistentNaming
        public string GetWindowsManagementInstrumentationValue(string query, string wmiClass, string failMessage){
            if (_osAnalyzer.IsWindows())
            {
                try
                {
                    var result = _processManager.RunPowerShellCommand($"/c Get-WmiObject -" + query + "'SELECT * FROM meta_class WHERE __class = '" + wmiClass);

                    result = result.Replace(wmiClass, String.Empty).Replace(" ", String.Empty);
                    
                    return result;
                }
                catch{
                    return failMessage;
                }
            }

            throw new PlatformNotSupportedException();
        }
}