using System;
using System.Globalization;
using System.Threading;

namespace Rpg.Client
{
    using Autofac;

    using Microsoft.Xna.Framework;

    using Rpg.Client.Core;
    using Rpg.Client.Engine;
    using Rpg.Client.Models;
    using Rpg.Client.Models.Biom;
    using Rpg.Client.Models.Combat;
    using Rpg.Client.Models.EndGame;
    using Rpg.Client.Models.Event;
    using Rpg.Client.Models.Map;
    using Rpg.Client.Models.Title;
    using Rpg.Client.Screens;

    public static class Program
    {
        private static IContainer ServiceProvider { get; set; }

        private static void ConfigureServices()
        {
            var services = new ContainerBuilder();

            GetMonogameDependencies(services);

            RegisterScreens(services);

            services.RegisterType<LinearDice>().As<IDice>();
            services.RegisterType<AnimationManager>();
            services.RegisterInstance(CreateGlobe());
            services.RegisterType<DialogContext>().As<IDialogContext>();

            ServiceProvider = services.Build();
        }

        private static Globe CreateGlobe()
        {
            return new Globe
            {
                Player = new Player
                {
                    Group = new Group
                    {
                        Units = new[]
                        {
                            new Unit(UnitSchemeCatalog.SwordmanHero, 1)
                            {
                                IsPlayerControlled = true
                            }
                        }
                    }
                }
            };
        }

        private static void GetMonogameDependencies(ContainerBuilder services)
        {
            var uiContentStorage = new UiContentStorage();
            var gameContentStorage = new GameObjectContentStorage();
            var game = new EwarGame(uiContentStorage, gameContentStorage);
            game.RunOneFrame();
            services.RegisterInstance(game).As<Game>().PropertiesAutowired();
            services.RegisterInstance(uiContentStorage).As<IUiContentStorage>();
            services.RegisterInstance(gameContentStorage).As<GameObjectContentStorage>();
            services.RegisterInstance(game.GraphicsDeviceManager);
            services.RegisterInstance(game.GraphicsDevice);
        }

        [STAThread]
        private static void Main()
        {
            var defaultCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;

            ConfigureServices();
            using var game = ServiceProvider.Resolve<Game>();
            game.Run();
        }

        private static void RegisterScreens(ContainerBuilder services)
        {
            services.RegisterType<ScreenManager>()
                    .As<IScreenManager>()
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            services.RegisterType<TitleScreen>().InstancePerLifetimeScope();
            services.RegisterType<MapScreen>().InstancePerLifetimeScope();
            services.RegisterType<EventScreen>().InstancePerLifetimeScope();
            services.RegisterType<EndGameScreen>().InstancePerLifetimeScope();
            services.RegisterType<CombatScreen>().InstancePerLifetimeScope();
            services.RegisterType<BiomScreen>().InstancePerLifetimeScope();
        }
    }
}