using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Media.Imaging;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using MahApps.Metro.Controls;
using NUnit.Framework;

using StockApp;
using AutomationElement = FlaUI.Core.AutomationElements.AutomationElement;

namespace StockAppTest
{
    [TestFixture]
    public class SmokeTestUI
    {
        private Window _window;
        private Stopwatch _stop;
        private Application _app;
        private ConditionFactory _cF;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan QuickTimeout = TimeSpan.FromSeconds(3);

        [SetUp]
        public void Setup()
        {
            _stop = Stopwatch.StartNew();
            _app = Application.Launch("StockApp.exe");
            using var auto = new UIA3Automation();
            _window = _app.GetMainWindow(auto);
            _cF = new ConditionFactory(new UIA3PropertyLibrary());
        }

        [TearDown]
        public void TearDown()
        {
            _stop.Stop();
            _app.Close();
        }

        [Test]
        public void TC1_1_StartApp()
        {
            Assert.That(_app, Is.Not.Null);
        }

        [Test]
        public void TC1_2_MainWindowUI()
        {            
            Assert.That(_window, Is.Not.Null);
        }

        [Test]
        public void TC1_3_StockScreenerUI()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            Assert.That(stockTab.IsSelected);
        }

        [Test]
        public void TC1_4_StockScreenerDisplayTopResultsNasdaq100()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(1).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC1_5_StockScreenerDisplayBottomResultsNasdaq100()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("OnToggle")).AsToggleButton().Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(1).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC1_6_StockScreenerDisplayTopResultsDow30()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(0).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC1_7_StockScreenerDisplayBottomResultsDow30()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("OnToggle")).AsToggleButton().Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(0).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC1_8_StockScreenerDisplayTopResultsSP500()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(2).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC1_9_StockScreenerDisplayBottomResultsSP500()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("OnToggle")).AsToggleButton().Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(2).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC2_1_StockMainGetQuote()
        {
            _window.FindFirstDescendant(_cF.ByAutomationId("Ticker")).AsTextBox().Click();
            Keyboard.Type("AMD");
            _window.FindFirstDescendant(_cF.ByAutomationId("Go")).AsButton().Click();
            Wait.UntilInputIsProcessed(QuickTimeout);
            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("TdStockListView")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC2_2_StockBrowseTabStockScreenerBackToMain()
        {
            var stockTab = _window.FindFirstDescendant(_cF.ByText("Stock Screener")).AsTabItem().Select();
            stockTab.Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("OnToggle")).AsToggleButton().Click();
            _window.FindFirstDescendant(_cF.ByAutomationId("MoversGrp")).AsComboBox().Select(2).Click();

            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("MoversList")).AsListBox().Items;
            Wait.UntilInputIsProcessed(QuickTimeout);
            Assert.That(stockData, Is.Not.Null);

            var homeTab = _window.FindFirstDescendant(_cF.ByText("Home")).AsTabItem().Select();
            homeTab.Click();

            _window.FindFirstDescendant(_cF.ByAutomationId("Ticker")).AsTextBox().Click();
            Keyboard.Type("AMD");
            _window.FindFirstDescendant(_cF.ByAutomationId("Go")).AsButton().Click();
            Wait.UntilInputIsProcessed(QuickTimeout);
            var stockData2 = _window.FindFirstDescendant(_cF.ByAutomationId("TdStockListView")).AsListBox().Items;
            Assert.That(stockData2, Is.Not.Null);
        }

        [Test]
        public void TC2_3_StockMainPageIsNotNull()
        {
            var stockData = _window.FindFirstDescendant(_cF.ByAutomationId("TdStockListView")).AsListBox().Items;
            Assert.That(stockData, Is.Not.Null);
        }

        [Test]
        public void TC2_4_API_TDAmeritradeReceivesResponse()
        {
            _window.FindFirstDescendant(_cF.ByAutomationId("Ticker")).AsTextBox().Click();
            Keyboard.Type("MSFT");
            _window.FindFirstDescendant(_cF.ByAutomationId("Go")).AsButton().Click();
            Wait.UntilInputIsProcessed(QuickTimeout);
            var stockData2 = _window.FindFirstDescendant(_cF.ByAutomationId("TdStockListView")).AsListBox().Items;
            Assert.That(stockData2, Is.Not.Null);
        }

        [Test]
        public void TC_2_5_PrivacyUIOpen()
        {
            _window.FindFirstDescendant(_cF.ByAutomationId("Abt")).AsButton().Click();

            using var auto = new UIA3Automation();
            var privacyWindow = _app.GetAllTopLevelWindows(auto);
            
            Assert.That(privacyWindow[0], Is.Not.Null);
        }
    }
}