using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace RedditApi.Services.Services
{
    public class MyLogger : Logger, IMyLogger
    {
        public Logger Logger()
        {
            var config = new ConfigurationBuilder()
           .SetBasePath(System.IO.Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
            var logger = LogManager.GetCurrentClassLogger();
            return logger;
        }
    }
}
