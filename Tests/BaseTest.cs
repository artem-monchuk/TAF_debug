using OpenQA.Selenium;
using Framework.Business.Pages;
using Framework.Core.Browser;
using Framework.Core.Utilities;
using Framework.Tests.Configuration;
using NUnit.Framework.Interfaces;

namespace Framework.Tests
{
    [TestFixtureSource(nameof(GetBrowsers))]
    public abstract class BaseTest
    {
        //dynamically load browsers from configuration
        protected static IEnumerable<string> GetBrowsers()
        {
            return ConfigurationManager.Instance.Settings.Browsers ?? throw new ArgumentNullException("Browsers list is empty!");
        }

        private readonly string _browser;
        protected IWebDriver Driver => DriverHandler.Instance.GetDriver();
        protected TestSettings TestSettings => ConfigurationManager.Instance.Settings;

        //pass the browser name to the constructor from the text fixture source
        protected BaseTest(string browser)
        {
            _browser = browser;
        }

        //OneTimeSetUp attribute to explicitly create artifact folders, including in CI/CD runner
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //create artifact folders once per class
            EnsureArtifactFoldersExist();
        }

        private void EnsureArtifactFoldersExist()
        {
            string logsDir = Path.Combine(GetProjectRootDirectory(), "Logs");
            string screenshotsDir = Path.Combine(GetProjectRootDirectory(), "Screenshots");
        
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);
        
            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);
        }

        protected const int DEFAULT_TIMEOUT = 10;
        protected const int EXTENDED_TIMEOUT = 30;
        protected const int SHORT_TIMEOUT = 5;

        protected HomePage HomePage;
        protected SearchResultsPage SearchResultsPage;
        protected CareersPage CareersPage;
        protected JobDetailsPage JobDetailsPage;
        protected InsightsPage InsightsPage;
        protected ArticlePage ArticlePage;
        protected AboutPage AboutPage;
        protected virtual bool RequiresDownloadConfiguration => false;
        protected virtual bool IsHeadless => false;

        [SetUp]
        public void Setup()
        {
            TestSettings.Browser = _browser;
            Logger.ConfigureLogging(TestSettings);
            Logger.LogInfo($"Starting test on {_browser}: {TestContext.CurrentContext.Test.Name}");

            DriverHandler.Instance.InitializeDriver(TestSettings);

            Driver.Manage().Cookies.DeleteAllCookies();
            InitializePages();
            NavigateToHomePage();
        }

        private void InitializePages()
        {
            HomePage = new HomePage(Driver);
            SearchResultsPage = new SearchResultsPage(Driver);
            CareersPage = new CareersPage(Driver);
            JobDetailsPage = new JobDetailsPage(Driver);
            InsightsPage = new InsightsPage(Driver);
            ArticlePage = new ArticlePage(Driver);
            AboutPage = new AboutPage(Driver);
        }

        protected void NavigateToHomePage()
        {
            Driver.Navigate().GoToUrl("https://www.epam.com/");
            Driver.WaitUntilPageLoaded();

            HomePage.AcceptCookies();

            Logger.LogInfo("Navigated to EPAM homepage");
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                CaptureScreenshot();
            }
            DriverHandler.Instance.QuitDriver();
        }

        private void CaptureScreenshot()
        {
            string screenshotsDir = Path.Combine(GetProjectRootDirectory(), "Screenshots");
            Directory.CreateDirectory(screenshotsDir); // ensures it exists
        
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string testName = TestContext.CurrentContext.Test.Name;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                testName = testName.Replace(c, '_');
            }
        
            string filePath = Path.Combine(screenshotsDir, $"{testName}_{timestamp}.png");
        
            Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            File.WriteAllBytes(filePath, screenshot.AsByteArray);
        
            Logger.LogError($"Screenshot saved: {filePath}");
        }

        private string GetProjectRootDirectory()
        {
            var dir = AppContext.BaseDirectory;
            while (Directory.GetParent(dir) != null)
            {
                if (Directory.EnumerateFiles(dir, "*.sln").Any() || Directory.Exists(Path.Combine(dir, ".git")))
                    return dir;
        
                dir = Directory.GetParent(dir)!.FullName;
            }
        
            throw new DirectoryNotFoundException("Could not determine project root directory.");
        }
    }
}
