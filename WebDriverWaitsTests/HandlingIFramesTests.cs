using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebDriverWaitsTests
{
    public class HandlingIFramesTests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://codepen.io/pervillalva/full/abPoNLd");
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
        public void FrameByIndexTests()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.TagName("iframe")));

            var dropdownBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".dropbtn")));
            dropdownBtn.Click();

            var dropdownContent = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".dropdown-content a")));

            foreach (var link in dropdownContent)
            {
               Console.WriteLine(link.Text);
               Assert.That(link.Displayed, Is.True, "Link was not displayed as expected.");
            }

            driver.SwitchTo().DefaultContent();
        }

        [Test, Order(2)]
        public void FrameByIDTests()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("result")));

            var dropdownBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".dropbtn")));
            dropdownBtn.Click();

            var dropdownContent = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".dropdown-content a")));

            foreach (var link in dropdownContent)
            {
                Console.WriteLine(link.Text);
                Assert.That(link.Displayed, Is.True, "Link was not displayed as expected.");
            }

            driver.SwitchTo().DefaultContent();
        }

        [Test, Order(3)]
        public void FrameByWebElementTests()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.CssSelector("#result")));

            var dropdownBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".dropbtn")));
            dropdownBtn.Click();

            var dropdownContent = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".dropdown-content a")));

            foreach (var link in dropdownContent)
            {
                Console.WriteLine(link.Text);
                Assert.That(link.Displayed, Is.True, "Link was not displayed as expected.");
            }

            driver.SwitchTo().DefaultContent();
        }
    }
}
