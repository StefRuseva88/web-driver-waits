using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebDriverWaitsTests
{
    public class HandlingAlertsTest
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test, Order(1)]
        public void HandleAlertTest()
        {
            driver.FindElement(By.XPath("//button[text()='Click for JS Alert']")).Click();

            IAlert alert = driver.SwitchTo().Alert();

            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "The alert message is not correct.");

            alert.Accept();

            IWebElement result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You successfully clicked an alert"), "The result message is not correct.");
        }

        [Test, Order(2)]
        public void HandleConfirmTest()
        {
            driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']")).Click();

            IAlert alert = driver.SwitchTo().Alert();

            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "The alert message is not correct.");

            alert.Accept();

            IWebElement result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You clicked: Ok"), "The result message is not correct.");

            driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']")).Click();

            alert = driver.SwitchTo().Alert();

            alert.Dismiss();

            result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You clicked: Cancel"), "The result message is not correct.");
        }

        [Test, Order(3)]
        public void HandlePromptTest()
        {
            driver.FindElement(By.XPath("//button[text()='Click for JS Prompt']")).Click();

            IAlert alert = driver.SwitchTo().Alert();

            Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "The alert message is not correct.");

            string inputText = "Hello, World!";
            alert.SendKeys(inputText);

            alert.Accept();

            IWebElement result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You entered: " + inputText), "The result message is not correct.");
        }
    }
}
