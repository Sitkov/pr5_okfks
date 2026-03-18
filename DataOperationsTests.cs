using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{
    public class DataOperationsTests : IDisposable
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

        // позитивные сценарии
        [Fact]
        public void Test1_PositiveScenario_CreateNote()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            string testTitle = "Тест" + DateTime.Now.Ticks; 
            _webDriver.FindElement(By.Id("noteTitle")).SendKeys(testTitle);
            _webDriver.FindElement(By.Id("noteContent")).SendKeys("Это текст позитивного сценария");

            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1500);

            var message = _webDriver.FindElement(By.Id("message"));
            Assert.DoesNotContain("hidden", message.GetAttribute("class"));
        }

        // негативные сценарии" 
        [Fact]
        public void Test2_NegativeScenario_MissingRequiredField()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            _webDriver.FindElement(By.Id("noteContent")).SendKeys("без заголовка");

            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1000);

            var hiddenId = _webDriver.FindElement(By.Id("noteId")).GetAttribute("value");
            Assert.Equal("", hiddenId); 
        }

        // повторные действия
        [Fact]
        public void Test3_RepeatedActions_CreateMultipleNotes()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);
            _webDriver.FindElement(By.Id("noteTitle")).SendKeys("1 повторная");
            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1000);

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);
            _webDriver.FindElement(By.Id("noteTitle")).SendKeys("2 повторная");
            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1000);

            var notesListHtml = _webDriver.FindElement(By.Id("notesList")).GetAttribute("innerHTML");
            Assert.Contains("1 повторная", notesListHtml);
            Assert.Contains("2 повторная", notesListHtml);
        }

        // работа с пустыми или неполными значениями
        [Fact]
        public void Test4_EmptyOrIncompleteValues_SubmitEmptyForm()
        {
            Login();

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);

            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1000);

            Assert.True(_webDriver.FindElement(By.Id("noteTitle")).Displayed);
        }

        // сохранение и обновление информации после выполнения действий
        [Fact]
        public void Test5_SaveAndUpdateInformation()
        {
            Login();

            string uniqueName = "Обновленная инфа " + DateTime.Now.Ticks;

            _webDriver.FindElement(By.Id("newNoteBtn")).Click();
            Thread.Sleep(500);
            _webDriver.FindElement(By.Id("noteTitle")).SendKeys(uniqueName);
            _webDriver.FindElement(By.Id("saveBtn")).Click();
            Thread.Sleep(1500);

            var notesList = _webDriver.FindElement(By.Id("notesList"));

            Assert.Contains(uniqueName, notesList.Text);
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}