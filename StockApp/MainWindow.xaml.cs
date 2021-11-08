using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.AppCenter.Crashes;
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
        public static bool TopOrBottom;
        private Privacy _privacyWindow;

        public static ObservableCollection<TDQuoteModel> TdStocks { get; set; }
        public static ObservableCollection<TDQuoteModel> TdDetails { get; set; }
        public static ObservableCollection<MoversModel> Movers { get; set; }
        public static string MoversSymbol { get; set; }
        public static double? MoversLastPrice { get; set; }
        public static double? ChangePct { get; set; }
        public static double? ChangeAfter { get; set; }
        public static double? ChangePre { get; set; }
        public static double? PrevClosePrice { get; set; }
        public static int? MoversVolume { get; set; }
        public static double? PctTenDayVol { get; set; }
        public static string GroupName { get; set; }

        public MainWindow()
        {
            FrameworkService.DotNetVersionCheck();
            InitializeComponent();
            TopOrBottom = true;
            ListSetup();
        }

        private void PrivacyWindow(object sender, RoutedEventArgs e)
        {
            if (_privacyWindow is { IsLoaded: true }) return;
            _privacyWindow = new Privacy()
            {
                Owner = this
            };

            Analytics.TrackEvent("Privacy Page Viewed");

            _privacyWindow.Show();
        }

        private void ListSetup()
        {
            ClearLists();

            if (TdStocks != null) return;
            TdStocks = new ObservableCollection<TDQuoteModel>();
            TdDetails = new ObservableCollection<TDQuoteModel>();
            Movers = new ObservableCollection<MoversModel>();

            TdStockListView.ItemsSource = TdStocks;
            TdStockListViewDetails.ItemsSource = TdDetails;
            MoversList.ItemsSource = Movers;
        }

        private static void ClearLists()
        {
            TdStocks?.Clear();
            TdDetails?.Clear();
            Movers?.Clear();
        }

        private static string ManageInputForCalc(string temp)
        {
            var ex = new Regex(@"[^0-9.]");
            temp = ex.Replace(temp, "");
            temp = temp.Trim();
            return temp;
        }

        #region HomePage

        private void GoButton(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Ticker.Text)) return;

            ManageInputForTicker();
            DataPull();
            DataContext = this;
        }

        private void ManageInputForTicker()
        {
            var temp = Ticker.Text;
            var ex = new Regex(@"[^a-zA-Z]");
            temp = ex.Replace(temp, "");
            temp = temp.Trim();
            Ticker.Text = temp;
            _symbol = temp;
        }

        private async void DataPull()
        {
            var controller = new TDController();

            ListSetup();
            ClearLists();

            try
            {
                controller.Initiate();
                await controller.GetQuoteAsync(_symbol).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return;
            }

            controller.TDQuoteAssign();
        }

        #endregion

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
            MoversList.ItemsSource = Movers;

        }

        private async void Movers_OnToggled(object sender, RoutedEventArgs e)
        {
            var controller = new CnbcController
            {
                Exchange = MoversGrp.SelectionBoxItem.ToString()
            };
            ClearLists();

            if (string.IsNullOrEmpty(controller.Exchange)) return;
            if (sender is not ToggleSwitch toggleSwitch) return;

            TopOrBottom = toggleSwitch.IsOn;
            await controller.GetMoversAsync();
            MoversList.ItemsSource = Movers;

        }

        private async void MoversRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            var controller = new CnbcController
            {
                Exchange = MoversGrp.SelectionBoxItem.ToString()
            };
            ClearLists();

            if (string.IsNullOrEmpty(controller.Exchange)) return;
            await controller.GetMoversAsync();
            MoversList.ItemsSource = Movers;

        }

        #endregion

        #region CalculationsPage

        private void PercentChangedBtn(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PercentBought.Text)) return;
            if (string.IsNullOrEmpty(PercentSold.Text)) return;

            double pB = 0;
            double pS = 0;

            try
            {
                var temp1 = ManageInputForCalc(PercentBought.Text);
                var temp2 = ManageInputForCalc(PercentSold.Text);
                PercentBought.Text = temp1;
                PercentSold.Text = temp2;
                pB = double.Parse(temp1);
                pS = double.Parse(temp2);
            }
            catch (FormatException exception)
            {
                Console.WriteLine(exception.Message);
            }

            var pR = ((pS - pB) / pB) * 100;
            const int maxLength = 5;
            var convResult = pR.ToString(CultureInfo.InvariantCulture);
            var convSubstring = convResult.Length > maxLength ? convResult.Substring(0, maxLength) : convResult;
            PercentResult.Text = convSubstring + "%";
        }

        private void Percent_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(PercentBought.Text)) return;
            if (string.IsNullOrEmpty(PercentSold.Text)) return;

            if (e.Key == Key.Return)
            {
                PercentChangedBtn(sender, e);
            }
        }

        #endregion
    }
}
