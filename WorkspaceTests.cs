using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace SeleniumTest
{
    public class WorkspaceTests : IDisposable
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

        // Отображение основной части интерфейса после входа
        [Fact]
        public void Test1_MainInterfaceDisplayAfterLogin()
        {
            Login();

            var notesSection = _webDriver.FindElement(By.Id("notesSection"));
            var toolbar = _webDriver.FindElement(By.ClassName("toolbar"));
            var notesLayout = _webDriver.FindElement(By.ClassName("notes-layout"));

            Assert.DoesNotContain("hidden", notesSection.GetAttribute("class"));
            Assert.True(toolbar.Displayed);
            Assert.True(notesLayout.Displayed);
        }

        // доступность базовых элементов управления
        [Fact]
        public void Test2_BasicControlElementsAvailability()
        {
            Login();

            var newNoteBtn = _webDriver.FindElement(By.Id("newNoteBtn"));
            var searchInput = _webDriver.FindElement(By.Id("searchInput"));
            var filterSelect = _webDriver.FindElement(By.Id("noteScopeFilter"));

            Assert.True(newNoteBtn.Displayed && newNoteBtn.Enabled);
            Assert.True(searchInput.Displayed && searchInput.Enabled);
            Assert.True(filterSelect.Displayed && filterSelect.Enabled);
        }

        // корректность переходов между рабочими состояниями
        [Fact]
        public void Test3_TransitionsBetweenWorkingStates()
        {
            Login();

            var newNoteBtn = _webDriver.FindElement(By.Id("newNoteBtn"));
            var editorTitle = _webDriver.FindElement(By.Id("editorTitle"));
            var noteTitleInput = _webDriver.FindElement(By.Id("noteTitle"));

            newNoteBtn.Click();
            Thread.Sleep(500);

            Assert.Equal("Новая заметка", editorTitle.Text);
            Assert.Equal("", noteTitleInput.GetAttribute("value"));
        }

        //изменение содержимого страницы в зависимости от действий пользователя
        [Fact]
        public void Test4_PageContentChangesDependingOnUserActions()
        {
            Login();

            var searchInput = _webDriver.FindElement(By.Id("searchInput"));
            var notesList = _webDriver.FindElement(By.Id("notesList"));

            string initialListHtml = notesList.GetAttribute("innerHTML");

            searchInput.SendKeys("фывйцуйцвываывацУАУЙКПЫФ");
            Thread.Sleep(1000); 

            string updatedListHtml = notesList.GetAttribute("innerHTML");
            Assert.NotEqual(initialListHtml, updatedListHtml);
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}