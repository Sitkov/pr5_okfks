using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{
    public class SearchAndFilterTests : IDisposable
    {
        IWebDriver _webDriver = new ChromeDriver();

        private void Login()
        {
            _webDriver.Url = "https://test.webmx.ru/";
            _webDriver.FindElement(By.Id("authUsername")).SendKeys("1010");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("!123qwe123!");
            _webDriver.FindElement(By.Id("authSubmit")).Click();
            Thread.Sleep(2000);
        }

        // находить нужные данные
        [Fact]
        public void Test1_FindSpecificData_UsingSearch()
        {
            Login();

            var searchInput = _webDriver.FindElement(By.Id("searchInput"));

            searchInput.SendKeys("йцукенгшщз");
            Thread.Sleep(1000); 

            var notesList = _webDriver.FindElement(By.Id("notesList"));

            Assert.DoesNotContain("йцукенгшщз", notesList.Text);
        }

        //ограничивать набор отображаемых объектов
        [Fact]
        public void Test2_RestrictDisplayedObjects_UsingFilters()
        {
            Login();

            var filterSelect = _webDriver.FindElement(By.Id("noteScopeFilter"));

            var optionMine = _webDriver.FindElement(By.XPath("//option[@value='mine']"));
            optionMine.Click();
            Thread.Sleep(1000);

            Assert.Equal("mine", filterSelect.GetAttribute("value"));
        }

        // менять состав видимых данных в зависимости от выбранных условий
        [Fact]
        public void Test3_ChangeVisibleDataComposition_BasedOnConditions()
        {
            Login();

            var searchInput = _webDriver.FindElement(By.Id("searchInput"));
            var filterSelect = _webDriver.FindElement(By.Id("noteScopeFilter"));
            var notesList = _webDriver.FindElement(By.Id("notesList"));

            string initialHtml = notesList.GetAttribute("innerHTML");

            var optionShared = _webDriver.FindElement(By.XPath("//option[@value='shared']"));
            optionShared.Click();
            searchInput.SendKeys("Тест");
            Thread.Sleep(1500);

            string updatedHtml = notesList.GetAttribute("innerHTML");
            Assert.NotEqual(initialHtml, updatedHtml);
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}