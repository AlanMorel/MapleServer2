using Autofac;
using Maple2.Data.Utils;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using Maple2.Sql.Context;
using Maple2.Sql.Model;
using Maple2.Data.Storage;
using System.Threading;
using Maple2.Data.Converter;
using Maple2.Data.Factory;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

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

        public static void RegisterModels(this ContainerBuilder builder)
        {
            // Register each Model
        }

        public static void RegisterStorage(this ContainerBuilder builder)
        {
            DBConnect dbconnect = new DBConnect();
            InitializationContext initContext = new InitializationContext(dbconnect.connectionString);
            initContext.Initialize();

            builder.RegisterType<UserStorage>()
                .AsSelf()
                .SingleInstance();
            
        }
    }
}
