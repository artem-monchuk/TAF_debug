using Serilog;
using Framework.Tests.Configuration;
using Serilog.Events;

namespace Framework.Core.Utilities
{
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance =
            new Lazy<Logger>(() => new Logger());

        private bool _isConfigured;

        private Logger() { }

        private static Logger Instance => _instance.Value;

        //use public methods to hide the internal implementation
        public static void LogInfo(string message)
        {
            Instance.LogInfoInternal(message);
        }

        public static void LogWarning(string message)
        {
            Instance.LogWarningInternal(message);
        }

        public static void LogError(string message)
        {
            Instance.LogErrorInternal(message);
        }

        //internal methods using Singleton instance
        private void LogInfoInternal(string message)
        {
            var testName = TestContext.CurrentContext?.Test?.Name ?? "Unknown";
            Log.Information($"[{testName}] {message}");
        }

        private void LogWarningInternal(string message)
        {
            var testName = TestContext.CurrentContext?.Test?.Name ?? "Unknown";
            Log.Warning($"[{testName}] {message}");
        }

        private void LogErrorInternal(string message)
        {
            var testName = TestContext.CurrentContext?.Test?.Name ?? "Unknown";
            Log.Error($"[{testName}] {message}");
        }

        public static void ConfigureLogging(TestSettings settings)
        {
            Instance.ConfigureLoggingInternal(settings);
        }

        private void ConfigureLoggingInternal(TestSettings settings)
        {
            if (_isConfigured) return;

            var logPath = settings.Logging.LogFilePath;

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Is(Enum.Parse<LogEventLevel>(settings.Logging.MinLogLevel, true))
                    .WriteTo.Console()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                    .CreateLogger();

            _isConfigured = true;

            LogInfo("Logging configured");
        }
    }
}

