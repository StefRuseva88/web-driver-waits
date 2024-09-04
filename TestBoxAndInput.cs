using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebDriverWaits
{
    public class TestBoxAndInput
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArguments("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test, Order(1)]
        public void AddBoxWithoutWaitsFails()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            IWebElement addBtn = driver.FindElement(By.XPath("//input[@id='adder']"));
            addBtn.Click();

            // Assert that NoSuchElementException is thrown when trying to find the new box without wait
            Assert.Throws<NoSuchElementException>(() =>
            {
                var redbox = driver.FindElement(By.XPath("//div[@class='redbox']"));
                Assert.That(redbox.Displayed, Is.True);
            });
        }

        [Test, Order(2)]
        public void RevealInputWithoutWaitsFail()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            IWebElement revealBtn = driver.FindElement(By.XPath("//input[@id='reveal']"));
            revealBtn.Click();

            // Assert that ElementNotInteractableException is thrown when trying to interact with the input without waits
            Assert.Throws<ElementNotInteractableException>(() =>
            {
                var revealedInput = driver.FindElement(By.XPath("//input[@id='revealed']"));
                revealedInput.SendKeys("Test");
            });
        }

        [Test, Order(3)]
        public void InputFieldInteractionsWithThreadSleep()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            driver.FindElement(By.XPath("//input[@id='adder']")).Click();
            Thread.Sleep(5000);

            var redbox = driver.FindElement(By.XPath("//div[@class='redbox']"));

            Assert.That(redbox.Displayed, Is.True);
        }

        [Test, Order(4)]
        public void InputFieldInteractionsWithImplicitWait()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.XPath("//input[@id='reveal']")).Click();

            IWebElement revealedInput = driver.FindElement(By.XPath("//input[@id='revealed']"));

            Assert.That(revealedInput.TagName, Is.EqualTo("input"));
        }

        [Test, Order(5)]
        public void InputFieldInteractionsWithExplicitWait()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            driver.FindElement(By.XPath("//input[@id='reveal']")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='revealed']")));

            var inputField = driver.FindElement(By.XPath("//input[@id='revealed']"));

            Assert.That(inputField.Displayed, Is.True);
        }

        [Test, Order(6)]
        public void AddBoxInteractionsWithFluentWait()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            IWebElement addBtn = driver.FindElement(By.XPath("//input[@id='adder']"));
            addBtn.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromMilliseconds(500);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            IWebElement newBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box0")));

            Assert.That(newBox.Displayed, Is.True);
        }

        [Test, Order(7)]
        public void RevealInputWithCustomFluentWait()
        {
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
            IWebElement revealed = driver.FindElement(By.Id("revealed"));
            driver.FindElement(By.Id("reveal")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };
            wait.IgnoreExceptionTypes(typeof(ElementNotInteractableException));

            wait.Until(d =>
            {
                revealed.SendKeys("Displayed");
                return revealed.GetAttribute("value") == "Displayed";
            });

            Assert.That(revealed.TagName, Is.EqualTo("input"));
            Assert.That(revealed.GetAttribute("value"), Is.EqualTo("Displayed"));
        }

        [Test, Order(8)]
        public void ExplicitWaits_ElementCreatedButNotVisible()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/1");
            driver.FindElement(By.XPath("//button[text()='Start']")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement helloWorld = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("finish")));

            Assert.That(helloWorld.Displayed, Is.True);
        }

        [Test, Order(9)]
        public void ImplicidWaits_ElementCreatedButNotVisible()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");
            driver.FindElement(By.XPath("//button[text()='Start']")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            IWebElement helloWorld = driver.FindElement(By.Id("finish"));

            Assert.That(helloWorld.Displayed, Is.True);
        }
    }
}