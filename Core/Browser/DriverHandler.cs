using OpenQA.Selenium;
using Framework.Tests.Configuration;

namespace Framework.Core.Browser
{
    public sealed class DriverHandler
    {
        private static readonly Lazy<DriverHandler> _instance =
            new Lazy<DriverHandler>(() => new DriverHandler());

        //singleton instance access
        public static DriverHandler Instance => _instance.Value;

        //a single global _driver field
        private IWebDriver _driver;

        //private constructor for Singleton
        private DriverHandler() { }

        public IWebDriver GetDriver()
        {
            //initialize the shared browser if it's not already initialized
            if (_driver == null)
            {
                var testSettings = ConfigurationManager.Instance.Settings;
                InitializeDriver(testSettings);
            }
            return _driver;
        }

        public void InitializeDriver(TestSettings settings)
        {
            QuitDriver();

            _driver = BrowserFactory.CreateDriver(
                settings.Browser,
                settings.ConfigureDownload,
                settings.Headless);
        }

        public void QuitDriver()
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Quit();
                }
                finally
                {
                    _driver.Dispose();
                    _driver = null; //allow initialization of a new browser instance
                }
            }
        }
    }
}
