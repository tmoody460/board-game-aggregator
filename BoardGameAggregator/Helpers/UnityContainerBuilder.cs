using BoardGameAggregator.Engines;
using BoardGameAggregator.Managers;
using BoardGameAggregator.Models;
using BoardGameAggregator.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameAggregator.Helpers
{
    public class UnityContainerBuilder
    {
        public static UnityContainer Build()
        {
            UnityContainer container = new UnityContainer();

            //Register Types
            container.RegisterType<SystemContext>(new PerResolveLifetimeManager());
            container.RegisterType<IBoardGameManager, BoardGameManager>(new PerResolveLifetimeManager());
            container.RegisterType<IBoardGameGeekInfoManager, BoardGameGeekInfoManager>(new PerResolveLifetimeManager());
            container.RegisterType<IRestApiEngine, RestApiEngine>(new PerResolveLifetimeManager());
            container.RegisterType<IBoardGameGeekEngine, BoardGameGeekEngine>(new PerResolveLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());

            return container;
        }
    }
}