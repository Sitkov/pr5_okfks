using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{
    public class UserAccountTests : IDisposable
    {
        IWebDriver _webDriver = new ChromeDriver();

        [Fact]
        public void Test1_LoginScenario()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("123123");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("123123");
            _webDriver.FindElement(By.Id("authSubmit")).Click();

            Thread.Sleep(2000);

            var welcomeText = _webDriver.FindElement(By.Id("welcomeText")).Text;
            Assert.Contains("123123", welcomeText);
        }

        [Fact]
        public void Test2_LogoutScenario()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("1010");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("!123qwe123!");
            _webDriver.FindElement(By.Id("authSubmit")).Click();
            Thread.Sleep(2000);

            _webDriver.FindElement(By.Id("logoutBtn")).Click();
            Thread.Sleep(1000);

            var authSection = _webDriver.FindElement(By.Id("authSection"));
            Assert.DoesNotContain("hidden", authSection.GetAttribute("class"));
        }

        [Fact]
        public void Test3_ReactionToErroneousActions()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("123123");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("wrong_password");
            _webDriver.FindElement(By.Id("authSubmit")).Click();

            Thread.Sleep(1000);

            var messageBox = _webDriver.FindElement(By.Id("message"));
            Assert.DoesNotContain("hidden", messageBox.GetAttribute("class"));
        }

        [Fact]
        public void Test4_StateChangeAfterSuccessfulAuth()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("1010");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("!123qwe123!");
            _webDriver.FindElement(By.Id("authSubmit")).Click();

            Thread.Sleep(2000);

            var authSection = _webDriver.FindElement(By.Id("authSection"));
            var notesSection = _webDriver.FindElement(By.Id("notesSection"));
            var userPanel = _webDriver.FindElement(By.Id("userPanel"));

            Assert.Contains("hidden", authSection.GetAttribute("class")); // Авторизация скрылась
            Assert.DoesNotContain("hidden", notesSection.GetAttribute("class")); // Заметки появились
            Assert.DoesNotContain("hidden", userPanel.GetAttribute("class")); // Панель юзера появилась
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}