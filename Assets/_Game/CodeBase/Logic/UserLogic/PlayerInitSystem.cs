using CodeBase._Services.StaticData;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.AimMove;
using CodeBase.Logic.Ecs;
using CodeBase.Logic.HealthLogick;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Logic.UserLogic
{
    public class PlayerInitSystem : IProtoInitSystem
    {
        private readonly GameAspect _aspect;
        private readonly GameFactory _gameFactory;
        private StaticDataService _staticData;

        public PlayerInitSystem(GameAspect aspect, GameFactory gameFactory, StaticDataService staticData)
        {
            _staticData = staticData;
            _gameFactory = gameFactory;
            _aspect = aspect;
        }
        
        public void Init(IProtoSystems systems)
        {
            var playerSettings = _staticData.GetPlayerData();
            var aimSettings = _staticData.GetAimData();
            var playerEntity = _aspect.World().NewEntity();
            ref Player player = ref _aspect.PlayerPool.Add(playerEntity);
            ref Health health = ref _aspect.Healths.Add(playerEntity);
            player.View = _gameFactory.GetPlayerView();
            health.Current = playerSettings.HitPoints;
            health.Max = playerSettings.HitPoints;
            
            ref AimTarget aimTarget = ref _aspect.AimTargets.NewEntity();
            aimTarget.TargetView = player.View.AimTargetView;
            aimTarget.DefaultSpeed = aimSettings.DefaultSpeed;
            aimTarget.Speed = aimSettings.DefaultSpeed;
        }
    }
}
