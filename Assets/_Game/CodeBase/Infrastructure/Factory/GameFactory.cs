using System.Threading.Tasks;
using CodeBase._Services.PersistentGameData;
using CodeBase._Services.Randomizer;
using CodeBase._Services.StaticData;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
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

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : SaversHolder
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
        private RoadPieceView _roadPiecePrefab;
        private Level _level;
        private PlayerView _playerPrefab;
        private PlayerView _player;

        public GameFactory(AssetProvider assets, StaticDataService staticData, RandomService randomService,
            PersistentProgress persistentProgress, MediatorFactory mediatorFactory, LifetimeScope parentScope)
        {
            _parentScope = parentScope;
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgress = persistentProgress;
            _mediatorFactory = mediatorFactory;
        }

        public async UniTask WarmUp(Level level)
        {
            _level = level;
            GameObject roadGo = await _assets.Load<GameObject>(AssetAddress.RoadPiece);
            _roadPiecePrefab = roadGo.GetComponent<RoadPieceView>();
            
            GameObject playerGo = await _assets.Load<GameObject>(AssetAddress.PlayerView);
            _playerPrefab = playerGo.GetComponent<PlayerView>();
        }

        public void Cleanup()
        {
            
        }

        public RoadPieceView GetRoadPiece(int index, float pieceLength)
        {
            RoadPieceView roadPiece = Object.Instantiate(_roadPiecePrefab, _level.RoadParent);
            roadPiece.Position =  -_level.RoadParent.forward * index * pieceLength;
            return roadPiece;
        }
        
        public PlayerView GetPlayerView()
        {
            if (_player == default) _player = Object.Instantiate(_playerPrefab, _level.PlayerSpawnPoint);
            return _player;
        }
    }
}
