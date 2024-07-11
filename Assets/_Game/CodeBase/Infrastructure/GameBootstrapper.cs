using System.Collections.Generic;
using CodeBase._Services;
using CodeBase._Services.Ads;
using CodeBase._Services.Input;
using CodeBase._Services.PersistentGameData;
using CodeBase._Services.Randomizer;
using CodeBase._Services.SaveLoad;
using CodeBase._Services.StaticData;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.UI.Mediators;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : LifetimeScope
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _loadingCurtain.Show();
            DontDestroyOnLoad(gameObject);
            builder.RegisterInstance(_loadingCurtain);
            builder.RegisterInstance(InputService());
            builder.Register<AssetProvider>(Lifetime.Singleton);
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<LoadProgressService>(Lifetime.Singleton);
            builder.Register<StaticDataService>(Lifetime.Singleton);
            builder.Register<PersistentProgress>(Lifetime.Singleton);
            builder.Register<RandomService>(Lifetime.Singleton);
            builder.Register<AdsService>(Lifetime.Singleton);
            builder.Register<EcsGameBuilder>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<GameFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<MediatorFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<SaveProgressService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LoadMenuState>(Lifetime.Singleton);
            builder.Register<LoadGameplayState>(Lifetime.Singleton);
            builder.Register<GameLoopState>(Lifetime.Singleton); 
            builder.Register<MenuState>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Game>();
        }
        
        private static IInputService InputService() => Application.isEditor
            ? new StandaloneInputService()
            : new MobileInputService();
    }
}
