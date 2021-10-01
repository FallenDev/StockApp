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
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1);

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
    }
}