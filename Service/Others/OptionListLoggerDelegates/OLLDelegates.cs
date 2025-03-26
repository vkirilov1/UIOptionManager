using Microsoft.Extensions.Logging;
using Service.Loggers;

namespace Service.Others.OptionListLoggerDelegates
{
    public class OLLDelegates
    {
        public static readonly OptionListLogger logger = new("Logger");

        public static void LogError(string error)
        {
            logger.LogError(error);
        }

        public static void LogInformation(string information)
        {
            logger.LogInformation(information);
        }
    }
}
