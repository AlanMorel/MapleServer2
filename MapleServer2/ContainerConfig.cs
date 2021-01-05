using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace MapleServer2
{
    public static class ContainerConfig
    {
        public static void RegisterLogger(this ContainerBuilder builder)
        {
            builder.Register((c) =>
            {
                var factory = new LoggerFactory();
                factory.AddProvider(new NLogLoggerProvider());
                return factory;
            })
                .As<ILoggerFactory>()
                .SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
        }
    }
}
