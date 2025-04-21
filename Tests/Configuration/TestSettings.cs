namespace Framework.Tests.Configuration
{
    public class TestSettings
    {
        public TestSettings() { }
        public required string Browser { get; set; }
        public required string[] Browsers { get; set; }
        public bool Headless { get; set; }
        public bool ConfigureDownload { get; set; }
        public LoggingSettings Logging { get; set; } = new LoggingSettings();
        public ApiSettings Api { get; set; } = new ApiSettings();
    }
    public class LoggingSettings
    {
        public string MinLogLevel { get; set; } = "Information";
        public string LogFilePath { get; set; } = "Logs/test_log.txt";
        public bool LogToConsole { get; set; } = true;
        public bool LogToFile { get; set; } = true;
    }
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "";
    }
}
