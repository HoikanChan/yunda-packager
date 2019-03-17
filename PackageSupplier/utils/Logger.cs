using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace PackageSupplier.utils
{
    class Logger
    {
        public static void WriteLog_info(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Info(msg);
        }
    }
}
