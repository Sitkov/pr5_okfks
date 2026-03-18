using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{
    public class FeedbackMessagesTests : IDisposable
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

        // наличие сообщений об успешном выполнении операций
        [Fact]
        public void Test1_PresenceOfSuccessMessages()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);
            _webDriver.FindElement(By.Id("noteTitle")).SendKeys("Успешная операция " + DateTime.Now.Ticks);
            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1000);

            // Проверяем, что блок сообщений появился 
            var messageBox = _webDriver.FindElement(By.Id("message"));
            Assert.DoesNotContain("hidden", messageBox.GetAttribute("class"));
        }

        // наличие сообщений об ошибках
        [Fact]
        public void Test2_PresenceOfErrorMessages()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("222222222222222222222");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("123123");
            _webDriver.FindElement(By.Id("authSubmit")).Click();
            Thread.Sleep(1000);

            var messageBox = _webDriver.FindElement(By.Id("message"));
            Assert.DoesNotContain("hidden", messageBox.GetAttribute("class"));
        }

        // понятность и однозначность сообщений
        [Fact]
        public void Test3_ClarityAndUnambiguityOfMessages()
        {
            _webDriver.Url = "https://test.webmx.ru/";

            _webDriver.FindElement(By.Id("authUsername")).SendKeys("invalid_user_999");
            _webDriver.FindElement(By.Id("authPassword")).SendKeys("wrong_pass");
            _webDriver.FindElement(By.Id("authSubmit")).Click();
            Thread.Sleep(1000);

            var messageBox = _webDriver.FindElement(By.Id("message"));
            string messageText = messageBox.Text;

            Assert.False(string.IsNullOrEmpty(messageText), "Сообщение об ошибке пустое!");
            Assert.True(messageText.Length > 5, "Текст сообщения слишком короткий и непонятный");
        }

        // корректность реакции интерфейса на подтверждение или отмену действий
        [Fact]
        public void Test4_CorrectInterfaceReaction_ToActionCancellation()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);
            _webDriver.FindElement(By.Id("noteTitle")).SendKeys("Текст123123");

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            var titleInput = _webDriver.FindElement(By.Id("noteTitle"));
            Assert.Equal("", titleInput.GetAttribute("value"));
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}