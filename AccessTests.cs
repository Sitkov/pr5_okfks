using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{
    public class AccessTests : IDisposable
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

        // какие действия доступны владельцу данных
        [Fact]
        public void Test1_ActionsAvailableToDataOwner()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            var saveBtn = _webDriver.FindElement(By.Id("saveBtn"));
            Assert.True(saveBtn.Displayed);
            Assert.True(saveBtn.Enabled);
        }

        // какие действия ограничены
        [Fact]
        public void Test2_RestrictedActions_ForNewUnsavedData()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            var deleteBtn = _webDriver.FindElement(By.Id("deleteBtn"));
            Assert.Equal("true", deleteBtn.GetAttribute("disabled"));
        }

        //изменяется ли интерфейс при переходе между ролями или пользовательскими состояниями
        [Fact]
        public void Test3_InterfaceChanges_OnStateTransition()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            var shareBlock = _webDriver.FindElement(By.Id("shareBlock"));
            Assert.Contains("hidden", shareBlock.GetAttribute("class"));
        }

        // можно ли через интерфейс обнаружить ошибочное предоставление избыточных возможностей
        [Fact]
        public void Test4_CheckForExcessivePrivileges()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            var shareBtn = _webDriver.FindElement(By.Id("shareBtn"));

            Assert.False(shareBtn.Displayed);
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}