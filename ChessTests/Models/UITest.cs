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
            _driver.Navigate().GoToUrl("https://localhost:7193/");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TestMethod]
        public void StartGameAndMovePieceTest()
        {
            // Indtast spiller-navne
            _driver.FindElement(By.Id("name1")).SendKeys("Alice");
            var player1Color = new SelectElement(_driver.FindElement(By.Id("player1Color")));
            player1Color.SelectByValue("lightblue");

            _driver.FindElement(By.Id("name2")).SendKeys("Bob");
            var player2Color = new SelectElement(_driver.FindElement(By.Id("player2Color")));
            player2Color.SelectByValue("maroon");

            // Start spillet
            _driver.FindElement(By.Id("beginGame")).Click();

            //// Vent lidt for navigation
            Thread.Sleep(1500);

            // Bekræft at vi er på /board siden
            Assert.IsTrue(_driver.Url.Contains("/board"));

            //WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _driver.FindElement(By.Id("a2")).Click();

            _driver.FindElement(By.Id("a3")).Click();

            Thread.Sleep(500); // lille vent for UI

            var a3 = _driver.FindElement(By.Id("a3"));
            Assert.IsTrue(
                a3.FindElements(By.TagName("span")).Count > 0,
                "FEJL: Brikken står IKKE på a3 efter trækket!"
            );

            var a2 = _driver.FindElement(By.Id("a2"));
            Assert.IsFalse(
                a2.FindElements(By.TagName("span")).Count > 0,
                "FEJL: Brikken står STADIG på a2!"
            );
        }
    }
}
