using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SpecFlowPOC.Utilities;
using TechTalk.SpecFlow;

namespace SpecFlowPOC.Hooks
{
    [Binding]
    public sealed class StartEndHook : ExtentReport
    {
        private readonly IObjectContainer _container;
        public StartEndHook(IObjectContainer container) 
        {
            _container = container;
        }

        [BeforeTestRun]
        public static void BeforeTestRun() 
        {
            ExtentReportInit();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ExtentReportTearDown();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            Console.WriteLine("Running After Feature.");
        }


        [BeforeScenario("@YoutubeSearch")]
        public void BeforeScenarioWithTag()
        {
            Console.WriteLine("Running inside the Youtube Search Tag");
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario(ScenarioContext scenarioContext)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            _container.RegisterInstanceAs<IWebDriver>(driver);
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _container.Resolve<IWebDriver>();

            if(driver != null)
            {
                driver.Quit();
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            var driver = _container.Resolve<IWebDriver>();

            if (scenarioContext.TestError == null)
            {
                if(stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepName);
                }
                else if(stepType == "When")
                {
                    _scenario.CreateNode<When>(stepName);
                }
                else if(stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepName);
                }
            }
            else if(scenarioContext.TestError != null)
            {
                if(stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build());
                }
                else if(stepType == "When")
                {
                    _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build());
                }
                else if(stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build());
                }
            }
        }
    }
}