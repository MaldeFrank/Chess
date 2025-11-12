using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ChessTests.Models
{
    [TestClass]

    public class UITest
    {
        private static readonly string DriverDirectory = "C:\\Driver";
        private static IWebDriver _driver;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _driver = new ChromeDriver(DriverDirectory);
            _driver.Navigate().GoToUrl("");
        }

        [TestMethod]
        public void StartGameTest()
        {
        }

        [TestMethod]
        public void MovePieceTest()
        {
        }
    }
}
