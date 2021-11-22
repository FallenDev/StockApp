using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace StockApp.Services
{
    public class FrameworkService
    {
        internal static bool DotNetVersionCheck()
        {
            var netVersion = Environment.Version;
            var net = netVersion.Major.ToString();
            var netMin = netVersion.Minor.ToString();
            var netMerge = $"{net}.{netMin}";
            return netMerge == "6.0" || DotNetPrompt();
        }

        private static bool DotNetPrompt()
        {
            var url = "https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-6.0.100-windows-x64-installer";
            if (MessageBox.Show(
                "We have detected that you do not meet the minimum framework requirement. Do you want to download the required framework? (.NET 6.0.100)",
                "StockApp", MessageBoxButton.YesNo, MessageBoxImage.Hand) != MessageBoxResult.Yes) return false;
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
            return false;
        }
    }
}
