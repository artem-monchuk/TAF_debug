using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace Framework.Core.Browser
{
    public class BrowserFactory
    {
        public static IWebDriver CreateDriver(string browserType, bool configureDownload, bool headless)
        {
            return browserType.ToLower() switch
            {
                "chrome" => CreateChromeDriver(configureDownload, headless),
                "firefox" => CreateFirefoxDriver(configureDownload, headless),
                "edge" => CreateEdgeDriver(configureDownload, headless),
                _ => throw new ArgumentException($"Browser {browserType} is not supported.")
            };
        }

        private static IWebDriver CreateChromeDriver(bool configureDownload, bool headless)
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            //added arguments for CI/CD pipeline
            options.AddArgument("--no-sandbox"); //to run in a restricted env as a Docker container or a CI/CD pipeline
            options.AddArgument("--incognito");
            options.AddArgument("--disable-dev-shm-usage");  //to overcome limited resource problems when running Chrome in Docker

            if (headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--window-size=1920,1080");
                options.AddArgument("--disable-gpu");
            }

            if (configureDownload)
            {
                var downloadPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads");
                options.AddUserProfilePreference("download.default_directory", downloadPath);
            }
            return new ChromeDriver(options);
        }
        private static IWebDriver CreateFirefoxDriver(bool configureDownload, bool headless)
        {
            var options = new FirefoxOptions();
            //options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--incognito");
            options.AddArgument("--disable-dev-shm-usage");

            if (headless)
            {
                options.AddArgument("--headless");
                //options.AddArgument("--window-size=1920,1080");
            }

            if (configureDownload)
            {
                var downloadPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads");
                //options.AddUserProfilePreference("download.default_directory", downloadPath);
            }

            var driver = new FirefoxDriver(options);
            //set window position to top left corner
            driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
            //add forced window size
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            return driver;
        }

        private static IWebDriver CreateEdgeDriver(bool configureDownload, bool headless)
        {
            var options = new EdgeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--incognito");
            options.AddArgument("--disable-dev-shm-usage");

            if (headless)
            {
                options.AddArgument("--headless");
                //options.AddArgument("--window-size=1920,1080");
            }
            if (configureDownload)
            {
                var downloadPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads");
                options.AddUserProfilePreference("download.default_directory", downloadPath);
            }
            return new EdgeDriver(options);
        }
    }
}
