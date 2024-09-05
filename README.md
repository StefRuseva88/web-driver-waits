# Selenium Waits Guide
[![C#](https://img.shields.io/badge/Made%20with-C%23-239120.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Google Chrome](https://img.shields.io/badge/tested%20on-Google%20Chrome-4285F4.svg)](https://www.google.com/chrome/)
[![NUnit](https://img.shields.io/badge/tested%20with-NUnit-22B2B0.svg)](https://nunit.org/)
[![Selenium](https://img.shields.io/badge/tested%20with-Selenium-43B02A.svg)](https://www.selenium.dev/)
### This is a test project for Front-End Test Automation July 2024 Course @ SoftUni
---

This repository provides detailed information and examples on how to handle waits in Selenium WebDriver, including the consequences of not using waits and the various types of waits you can use.

## Table of Contents

- [Introduction](#introduction)
- [Consequences of Not Using Waits](#consequences-of-not-using-waits)
- [Types of Waits in Selenium](#types-of-waits-in-selenium)
  - [Thread.sleep](#threadsleeptime)
  - [Implicit Wait](#implicit-wait)
  - [Explicit Wait](#explicit-wait)
  - [Fluent Wait](#fluent-wait)
- [Best Practices](#best-practices)
- [Examples](#examples)
- [Conclusion](#conclusion)
- [Contributing](#Contributing)
- [License](#License)
- [Contact](#Contact)
  
## Introduction

In Selenium WebDriver, waits are crucial for managing the timing issues that arise during test automation. Properly handling waits ensures that your tests run reliably and reduce the likelihood of errors related to synchronization.

## Consequences of Not Using Waits

Failing to use waits appropriately in your Selenium tests can lead to several issues:

1. **Flaky Tests:** Tests might fail intermittently because they are not waiting for elements to be available or actions to be completed.
2. **Timing Issues:** Actions may be attempted before elements are fully loaded, causing `NoSuchElementException` or `ElementNotVisibleException`.
3. **Inconsistent Test Results:** Tests may pass or fail unpredictably depending on the speed of the application and the machine running the tests.

## Types of Waits in Selenium

Selenium WebDriver provides different types of waits to handle various scenarios:

### Thread.sleep

- **Description:** Pauses the execution for a fixed amount of time.
- **Usage:** `Thread.sleep(milliseconds);`
- **Pros:** Simple to use.
- **Cons:** Inefficient as it does not account for the actual state of the element or the application. The test waits for the entire specified time, even if the element is available sooner.

### Implicit Wait

- **Description:** Sets a default wait time for the entire duration of the WebDriver instance.
- **Usage:** 
 ```csharp
  driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
 ```
- **Pros:** Automatically applies to all element lookups, reducing the need for explicit wait statements.
- **Cons:** Can lead to longer wait times for elements that may not need the full duration, and it's not always suitable for dynamic content.

### Explicit Wait
- **Description:** Waits for a specific condition to occur before proceeding with the test.
- **Usage:**
 ```csharp
 WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
 IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("elementId")));
 ```
- **Pros:** More flexible and precise, allowing you to wait for specific conditions such as visibility, presence, or clickability.
- **Cons:** Requires additional code and setup for each condition you need to wait for.
  
### Fluent Wait
- **Description:** Allows for more fine-grained control over the wait conditions, including polling intervals and ignoring specific exceptions.
- **Usage:**
 ```csharp
DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver)
{
    Timeout = TimeSpan.FromSeconds(10),
    PollingInterval = TimeSpan.FromSeconds(2)
};
fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

IWebElement element = fluentWait.Until(driver => driver.FindElement(By.Id("elementId")));
```
- **Pros:** Highly customizable, suitable for complex conditions and scenarios where you need to adjust polling frequency or handle specific exceptions.
- **Cons:** More complex to set up compared to implicit and explicit waits.

## Best Practices
1. Prefer Explicit Waits for Specific Conditions: Use explicit waits to handle specific cases where an element needs to meet a particular condition before interacting with it.
2. Use Implicit Waits for General Waiting: Set an implicit wait to cover general waiting scenarios, but be cautious about potential conflicts with explicit waits.
3. Avoid Overusing Thread.Sleep: Rely on Thread.Sleep sparingly and only for cases where other waits are not suitable.
4. Customize Fluent Waits for Complex Scenarios: Use fluent waits when you need more control over the waiting process, especially for dynamic content and polling intervals.

## Examples

### Example: Using Explicit Wait
```csharp
WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submitButton")));
element.Click();
```

### Example: Using Fluent Wait
```csharp
DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver)
{
    Timeout = TimeSpan.FromSeconds(15),
    PollingInterval = TimeSpan.FromSeconds(3)
};
fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

IWebElement element = fluentWait.Until(driver => driver.FindElement(By.Id("dynamicElement")));
```
## Conclusion
Using waits effectively is crucial for stable and reliable Selenium tests. By understanding the different types of waits and applying them appropriately, you can avoid common pitfalls related to timing issues and improve the overall quality of your test automation.

## Contributing
Contributions are welcome! If you have any improvements or bug fixes, feel free to open a pull request.

## License
This project is licensed under the [MIT License](LICENSE). See the [LICENSE](LICENSE) file for details.

## Contact
For any questions or suggestions, please reach out to the course instructor or open an issue in the repository.

---
### Happy Testing! 🚀
