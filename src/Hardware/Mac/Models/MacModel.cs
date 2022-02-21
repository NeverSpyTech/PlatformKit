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

using AluminiumTech.DevKit.PlatformKit.Hardware.Mac.Enums;
using AluminiumTech.DevKit.PlatformKit.Hardware.Shared;
using AluminiumTech.DevKit.PlatformKit.Hardware.Shared.Models;
using AluminiumTech.DevKit.PlatformKit.Software.Mac.Models;

namespace AluminiumTech.DevKit.PlatformKit.Hardware.Mac.Models;

/// <summary>
/// 
/// </summary>
public class MacModel
{
    public ReleaseYearModel ReleaseYear { get; set; }
    
    public MacDeviceFamily DeviceFamily { get; set; }
    
    public MacHardwareModel MacHardware { get; set; }
    
    public MacSupportStatus SupportStatus { get; set; }
    
    public MacOsSystemInformation InstalledOperatingSystem { get; set; }
}