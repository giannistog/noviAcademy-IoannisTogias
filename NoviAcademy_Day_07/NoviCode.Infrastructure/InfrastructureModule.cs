using Autofac;
using NoviCode.Infrastructure;
using NoviCode.Persistence.Commands.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode
{
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreatePlayerPersistence>().As<ICreatePlayerPersistence>().InstancePerLifetimeScope();

            builder.RegisterDecorator(typeof(CreatePlayersPersistenceCachingDecorator), typeof(ICreatePlayerPersistence));
        }
    }
}
