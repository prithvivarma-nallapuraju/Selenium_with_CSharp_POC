using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SpecFlowPOC.StepDefinitions
{
    [Binding]
    public class YoutubeSearchStepDefinitions
    {
        private IWebDriver driver;

        public YoutubeSearchStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
        }

        [Given(@"Open the Browser")]
        public void GivenOpenTheBrowser()
        {
            // driver = new ChromeDriver();
            // driver.Manage().Window.Maximize();
        }

        [When(@"Enter the Youtube URL")]
        public void WhenEnterTheYoutubeURL()
        {
            driver.Url = "https://www.youtube.com";
            Thread.Sleep(5000);
        }

        [Then(@"Search for the Selenium with (.*) tutorial")]
        public void ThenSearchForTheSeleniumWithCTutorial(string language)
        {
            driver.FindElement(By.XPath("//input[@name=\"search_query\"]")).Click();
            driver.FindElement(By.XPath("//input[@name=\"search_query\"]")).SendKeys("Selenium with " + language + " Tutorial");
            driver.FindElement(By.XPath("//input[@name=\"search_query\"]")).SendKeys(Keys.Enter);
            Thread.Sleep(5000);
        }

        [Then(@"Quit the browser")]
        public void ThenQuitTheBrowser()
        {
            // driver.Quit();
        }

    }
}