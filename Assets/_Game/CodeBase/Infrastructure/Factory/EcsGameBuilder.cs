using System.Threading.Tasks;
using CodeBase._Services.PersistentGameData;
using CodeBase._Services.Randomizer;
using CodeBase._Services.StaticData;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Logic.AimMove;
using CodeBase.Logic.Ecs;
using CodeBase.Logic.Road;
using CodeBase.Logic.TouchInput;
using CodeBase.Logic.UserLogic;
using CodeBase.UI.Mediators;
using Cysharp.Threading.Tasks;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Unity;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class EcsGameBuilder : SaversHolder
    {
        private readonly AssetProvider _assets;
        private readonly StaticDataService _staticData;
        private readonly RandomService _randomService;
        private readonly PersistentProgress _persistentProgress;
        private readonly MediatorFactory _mediatorFactory;
        private readonly LifetimeScope _parentScope;
        
        private ProtoWorld _world;
        private IProtoSystems _systems;
        private GameAspect _aspect;
        private LifetimeScope _gamePlayScope;
        private GameFactory _gameFactory;

        public IProtoSystems Systems => _systems;

        public EcsGameBuilder(AssetProvider assets, StaticDataService staticData, RandomService randomService,
            PersistentProgress persistentProgress, MediatorFactory mediatorFactory, LifetimeScope parentScope, GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _parentScope = parentScope;
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgress = persistentProgress;
            _mediatorFactory = mediatorFactory;
        }

        public async UniTask Build()
        {
            Level level = Object.FindAnyObjectByType<Level>();
            await _gameFactory.WarmUp(level);
            _aspect = new GameAspect();
            _world = new ProtoWorld(_aspect);
            
            _gamePlayScope = _parentScope.CreateChild(builder =>
            {
                builder.RegisterInstance(level);
                builder.Register<RoadRunSystem>(Lifetime.Singleton);
                builder.Register<UserTouchSystem>(Lifetime.Singleton);
                builder.Register<UserTouchSystemForEditor>(Lifetime.Singleton);
                builder.Register<AimTargetMoveSystem>(Lifetime.Singleton);
                builder.Register<PlayerInitSystem>(Lifetime.Singleton);
                builder.RegisterInstance(_aspect);
            });
          
            await ContainerBuild();
            
            _systems = new ProtoSystems(_world);
            _systems
                .AddModule(new UnityModule())
                .AddSystem(_gamePlayScope.Container.Resolve<RoadRunSystem>())
                .AddSystem(_gamePlayScope.Container.Resolve<PlayerInitSystem>())
                .AddSystem(_gamePlayScope.Container.Resolve<UserTouchSystemForEditor>())
                .AddSystem(_gamePlayScope.Container.Resolve<AimTargetMoveSystem>())
             // .AddSystem(_gamePlayScope.Container.Resolve<UserTouchSystem>())
                .Init();
        }
        
        private async Task ContainerBuild()
        {
            while (_gamePlayScope.Container == null)
                await UniTask.Yield();
        }

        public void Cleanup()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
            ProgressLoaders.Clear();
            ProgressSavers.Clear();
            _assets.Cleanup();
            _gamePlayScope.Dispose();
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
    }
}
