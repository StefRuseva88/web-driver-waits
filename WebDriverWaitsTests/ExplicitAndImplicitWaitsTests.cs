using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebDriverWaitsTests
{
    public class ExplicitAndImplicitWaitsTests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("http://practice.bpbonline.com");
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
        public void SearchForExistingProduct_ShouldAddToCart_Implicit()
        {
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("keyboard");

            var searchButton = driver.FindElement(By.XPath("//input[@title=' Quick Find ']"));
            searchButton.Click();

            try
            {
                driver.FindElement(By.XPath("//span[text()='Buy Now']")).Click();

                Assert.That(driver.PageSource.Contains("keyboard"), Is.True, "The product was not found.");
                Console.WriteLine("The Product was added successfully.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }
        }

        [Test, Order(2)]
        public void SearchForNonExistingProduct_ShouldThrowNoSuchElementException_Implicit()
        {
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("junk");

            var searchButton = driver.FindElement(By.XPath("//input[@title=' Quick Find ']"));
            searchButton.Click();

            try
            {
                driver.FindElement(By.XPath("//span[text()='Buy Now']")).Click();
            }
            catch (NoSuchElementException ex)
            {
                Assert.Pass("Expected NoSuchElementException was thrown");
                Console.WriteLine("Timeout - " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }
        }

        [Test, Order(3)]
        public void SearchForExistingProduct_ShouldAddToCart_Explicit()
        {
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("keyboard");

            var searchButton = driver.FindElement(By.XPath("//input[@title=' Quick Find ']"));
            searchButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement buyNowButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Buy Now']")));

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                buyNowButton.Click();

                Assert.That(driver.PageSource.Contains("keyboard"), Is.True, "The product was not found.");
                Console.WriteLine("The Product was added successfully.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }
        }

        [Test, Order(4)]
        public void SearchForNonExistingProduct_ShouldThrowNoSuchElementException_Explicit()
        {
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("junk");

            var searchButton = driver.FindElement(By.XPath("//input[@title=' Quick Find ']"));
            searchButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement buyNowButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Buy Now']")));

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                buyNowButton.Click();

                Assert.Fail("The 'BuyNow' Button product was found for non-existing product.");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Pass("Expected WebDriverTimeoutException was thrown");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
        }
    }
}