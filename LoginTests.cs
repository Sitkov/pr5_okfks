using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{ 
    public class LoginTests : IDisposable
    {
        IWebDriver _webDriver = new ChromeDriver();

        // Проверка стартового состояния приложения
        [Fact]
        public void CheckLoginVisible()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            var loginTab = _webDriver.FindElement(By.Id("loginTab"));

            Assert.Contains("active", loginTab.GetAttribute("class"));
        }

        // Переходы между режимами 
        [Fact]
        public void SwitchLoginAndRegTabs()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            var loginTab = _webDriver.FindElement(By.Id("loginTab"));
            var registerTab = _webDriver.FindElement(By.Id("registerTab"));
            var submitButton = _webDriver.FindElement(By.Id("authSubmit"));

            registerTab.Click();

            Assert.Contains("active", registerTab.GetAttribute("class"));
            Assert.Equal("Зарегистрироваться", submitButton.Text);

            loginTab.Click();

            Assert.Contains("active", loginTab.GetAttribute("class"));
            Assert.Equal("Войти", submitButton.Text);
        }

        // Поведение интерфейса при некорректных действиях
        [Fact]
        public void InvalidLogin_ShouldShowError()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("ab");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("123");
            _webDriver.FindElement(By.Id("authSubmit")).Click();

            Thread.Sleep(1000); 

            var authSection = _webDriver.FindElement(By.Id("authSection"));

            Assert.DoesNotContain("hidden", authSection.GetAttribute("class"));
        }

        // Успешный вход 
        [Fact]
        public void ValidLogin_ShowNotes()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("1010");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("!123qwe123!");
            _webDriver.FindElement(By.Id("authSubmit")).Click();

            Thread.Sleep(2000); 

            var authSection = _webDriver.FindElement(By.Id("authSection"));
            var notesSection = _webDriver.FindElement(By.Id("notesSection"));
            var userPanel = _webDriver.FindElement(By.Id("userPanel"));

            Assert.Contains("hidden", authSection.GetAttribute("class"));
            Assert.DoesNotContain("hidden", notesSection.GetAttribute("class"));
            Assert.DoesNotContain("hidden", userPanel.GetAttribute("class"));
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}