//using System;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Threading;
//using System.Windows.Media.Imaging;
//using FlaUI.Core;
//using FlaUI.Core.AutomationElements;
//using FlaUI.Core.Input;
//using FlaUI.TestUtilities;
//using FlaUI.UIA3;
//using NUnit.Framework;

//using StockApp;

//namespace StockAppTest
//{
//    [TestFixture]
//    public class SmokeTestUI : FlaUITestBase
//    {
//        private Window _window;
//        private Stopwatch _stop;

//        protected override AutomationBase GetAutomation()
//        {
//            return new UIA3Automation();
//        }

//        protected override Application StartApplication()
//        {
//            var app = Application.Launch("StockApp.exe");
//            app.WaitWhileMainHandleIsMissing();
//            return app;
//        }

//        [SetUp]
//        public void Setup()
//        {
//            _stop = Stopwatch.StartNew();
//            Mouse.Position = new Point(0, 0);
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _stop.Stop();
//            Console.WriteLine("Time elapsed: " + _stop.ElapsedMilliseconds);
//        }

//        [OneTimeTearDown]
//        public void FixtureTeardown()
//        {
//            Automation?.Dispose();
//            Application?.Close();
//        }

//        [Test]
//        public void SmokeTest_MainWindowUI()
//        {
//            _window = Application.GetMainWindow(Automation);
//            Assert.That(_window, Is.Not.Null);
//        }
//    }
//}