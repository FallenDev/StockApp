using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Microsoft.AppCenter.Analytics;
using StockApp.Controllers;
using StockApp.Models;
using StockApp.Services;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _symbol;

        internal static ObservableCollection<MoversModel> Movers { get; set; }
        public static string MoversSymbol { get; set; }
        public static double? MoversLastPrice { get; set; }
        public static double? ChangePct { get; set; }
        public static double? ChangeAfter { get; set; }
        public static double? ChangePre { get; set; }
        public static double? PrevClosePrice { get; set; }
        public static int? MoversVolume { get; set; }
        public static double? PctTenDayVol { get; set; }
        public static bool TopOrBottom { get; set; }
        public static string GroupName { get; set; }

        public MainWindow()
        {
            FrameworkService.DotNetVersionCheck();
            InitializeComponent();
            TopOrBottom = true;
            ListSetup();
        }

        private void ListSetup()
        {
            ClearLists();

            if (Movers != null) return;
            Movers = new ObservableCollection<MoversModel>();
            MoversList.ItemsSource = Movers;
        }

        private static void ClearLists()
        {
            Movers?.Clear();
        }

        #region MoversPage

        private void Movers_OpenModal(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void Movers_DropDownClosed(object sender, EventArgs e)
        {
            var controller = new CnbcController
            {
                Exchange = MoversGrp.SelectionBoxItem.ToString()
            };
            ClearLists();

            if (string.IsNullOrEmpty(controller.Exchange)) return;
            await controller.GetMoversAsync();
        }

        private async void Movers_OnToggled(object sender, RoutedEventArgs e)
        {
            var controller = new CnbcController();
            ClearLists();

            if (string.IsNullOrEmpty(controller.Exchange)) return;
            if (sender is not ToggleSwitch toggleSwitch) return;

            TopOrBottom = toggleSwitch.IsOn;
            await controller.GetMoversAsync();
        }

        private async void MoversRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            var controller = new CnbcController();
            ClearLists();

            if (string.IsNullOrEmpty(controller.Exchange)) return;
            await controller.GetMoversAsync();
        }

        #endregion
    }
}
