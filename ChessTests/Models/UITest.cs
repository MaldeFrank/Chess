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
using SeleniumExtras.WaitHelpers;


namespace ChessTests.Models
{
    [TestClass]

    public class UITest
    {
        private static readonly string DriverDirectory = "C:\\Driver";
        private static IWebDriver _driver;

        [ClassInitialize]
        public static void SetupAndStartGameTest(TestContext context)
        {
            _driver = new ChromeDriver(DriverDirectory);
            _driver.Navigate().GoToUrl("https://localhost:7193/startgame");

        }

        [TestMethod]
        public void StartGameAndMovePieceTest()
        {
            // Indtast spiller-navne
            _driver.FindElement(By.Id("name1")).SendKeys("Alice");
            _driver.FindElement(By.Id("name2")).SendKeys("Bob");

            // Start spillet
            _driver.FindElement(By.Id("beginGame")).Click();

            // Vent lidt for navigation
            Thread.Sleep(5000);

            // Bekræft at vi er på /board siden
            Assert.IsTrue(_driver.Url.Contains("/board"));


            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _driver.FindElement(By.Id("a2")).Click();

            _driver.FindElement(By.Id("a3")).Click();
        }
    }
}
