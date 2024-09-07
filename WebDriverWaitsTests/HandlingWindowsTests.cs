using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace WebDriverWaitsTests
{
    public class HandlingWindowsTests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/windows");
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
        public void HandleMultipleWindowsTest()
        {
           driver.FindElement(By.XPath("//a[text()='Click Here']")).Click();

            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

            Assert.That(windowHandles.Count, Is.EqualTo(2), "The new window was not opened.");
            driver.SwitchTo().Window(windowHandles[1]);

            string newWindowContent = driver.PageSource;
            Assert.That(newWindowContent, Does.Contain("New Window"), "The new window content is not correct.");

            string path = Path.Combine(Directory.GetCurrentDirectory(), "windows.txt");
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            File.AppendAllText(path, "Window handle for new window: " + driver.CurrentWindowHandle + "\n\n");

            driver.Close();

            driver.SwitchTo().Window(windowHandles[0]);

            string originalWindowContent = driver.PageSource;

            Assert.That(originalWindowContent.Contains("Opening a new window"), Is.True, "The original window content is not correct.");

            File.AppendAllText(path, "Window handle for original window: " + driver.CurrentWindowHandle + "\n\n");
            File.AppendAllText(path, "The page content " + originalWindowContent + "\n\n");
        }

        [Test, Order(2)]
        public void HandleNoSuchWindowsexceptionTest()
        {
            driver.FindElement(By.XPath("//a[text()='Click Here']")).Click();

            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;
            driver.SwitchTo().Window(windowHandles[1]);

            driver.Close();

            try
            {
                driver.SwitchTo().Window(windowHandles[1]);
            }
            catch (NoSuchWindowException ex)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "windows.txt");
                File.AppendAllText(path, "The exception message: " + ex.Message + "\n\n");
                Assert.Pass("Expected NoSuchWindowException was thrown");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }
            finally
            {
                driver.SwitchTo().Window(windowHandles[0]);
            }
        }
    }
}
