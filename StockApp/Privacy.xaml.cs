using System.Reflection;

using MahApps.Metro.Controls;

namespace StockApp
{
    public partial class Privacy : MetroWindow
    {
        public Privacy()
        {
            InitializeComponent();
            GetVersion();
        }

        private void GetVersion()
        {
            var version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            AppVersion.Text = version;
        }
    }
}
