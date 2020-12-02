using System.Reflection;
using Autofac;
using MapleServer2.Data;
using MapleServer2.Data.Static;
using MapleServer2.Network;
using MapleServer2.PacketHandlers;
using MapleServer2.Tools;

namespace MapleServer2.Servers.Game {
    public static class GameContainerConfig {
        public static IContainer Configure() {
            var builder = new ContainerBuilder();
            builder.RegisterLogger();
            builder.RegisterType<GameServer>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<PacketRouter<GameSession>>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<GameSession>()
                .AsSelf();
            builder.RegisterType<ManagerFactory<FieldManager>>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<StaticAccountStorage>()
                .As<IAccountStorage>()
                .SingleInstance();

            // Make all packet handlers available to PacketRouter
            builder.RegisterAssemblyTypes(Assembly.Load(nameof(MapleServer2)))
                .Where(t => typeof(IPacketHandler<GameSession>).IsAssignableFrom(t))
                .As<IPacketHandler<GameSession>>()
                .SingleInstance();

            return builder.Build();
        }
    }
}