using Newtonsoft.Json;

namespace Framework.Tests.Configuration
{
    public sealed class ConfigurationManager
    {
        //thread-safe Singleton implementation
        private static readonly Lazy<ConfigurationManager> _instance =
            new Lazy<ConfigurationManager>(() => new ConfigurationManager());

        private TestSettings? _settings;

        //private constructor for Singleton
        private ConfigurationManager() { }

        //public static property to access the instance
        public static ConfigurationManager Instance => _instance.Value;

        //public property to access settings
        public TestSettings Settings
        {
            get
            {

                if (_settings == null)
                {
                    LoadSettings();
                }

                return _settings ?? throw new InvalidOperationException("Settings could not be loaded.");
            }
        }

        private void LoadSettings()
        {
            var jsonContent = File.ReadAllText("Tests/Configuration/testsettings.json");
            _settings = JsonConvert.DeserializeObject<TestSettings>(jsonContent)
                ?? throw new InvalidOperationException("Failed to deserialize TestSettings.");

            //check if a browser override was passed in (from Jenkins or CLI)
            var browserOverride = TestContext.Parameters["browser"];
            if (!string.IsNullOrEmpty(browserOverride))
            {
                _settings.Browser = browserOverride;
            }
        }
    }
}
