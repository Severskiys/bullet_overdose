using System;
using CodeBase._Services.StaticData;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Logic.AimMove
{
    public class AimTargetMoveSystem : IProtoRunSystem
    {
        private readonly GameAspect _aspect;
        private StaticDataService _staticData;
        private AimData _aimData;
        private GameFactory _gameFactory;
        private AimTargetView _aim;

        public AimTargetMoveSystem(GameAspect aspect, StaticDataService staticData, GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _staticData = staticData;
            _aspect = aspect;
            _aimData = _staticData.GetAimData();
        }
        
        public void Run()
        {
          /*  if (_aspect.TouchsIt.IsEmpty())
                return;*/
            
            MoveAim(_aspect.TouchsIt.First());

            // в зависимости от инпути игрока - аим таргет двигается внутри определенного квадрата. Причем двигается его локальная позиция
            // существуют настройки границы движения для данного объекта, т.е. он не может покинуть определенняе границы своего местоположения
            // существует скорость движения, данная скорость максимальна, когда в приуеле игрка нет интерактивных объектов 
            // когда появляется интеравтивный объект, скорость движения прцела замедлается
            // когда происходит килл - скорость всей игры значительно замедляется.
            // скорость движения прицела замедляется не мнгонвенно, а постепенно
            // и ускоряется так же не сразу, но быстрее
        }

        private void MoveAim((ProtoEntity Entity, bool Ok) touchTuple)
        {
            if (touchTuple.Ok == false)
                return;
            
            var aimTuple = _aspect.AimTargetIt.First();
            if (aimTuple.Ok == false)
                return;
            
            var userTouch = _aspect.Touchs.Get(touchTuple.Entity);
            AimTarget aimTarget = _aspect.AimTargets.Get(aimTuple.Entity);
            
            if (userTouch.CurrentPhase != TouchPhase.Moved)
            {
                aimTarget.Velocity = Vector3.zero;
                return;
            }
            
            var current = aimTarget.TargetView.LocalPosition;
            var target = current + new Vector3(userTouch.Direction.x * _aimData.XSpeedModifier, userTouch.Direction.y * _aimData.YSpeedModifier, 0);
            ClampTarget(ref target);
            aimTarget.TargetView.LocalPosition = Vector3.SmoothDamp(current, target, ref aimTarget.Velocity, 1 / aimTarget.Speed);
        }

        private void ClampTarget(ref Vector3 target)
        {
            if (target.x > _aimData.XBorderMax)
                target.x = _aimData.XBorderMax;
            else if (target.x < _aimData.XBorderMin)
                target.x = _aimData.XBorderMin;
            
            if (target.y > _aimData.YBorderMax)
                target.y = _aimData.YBorderMax;
            else if (target.y < _aimData.YBorderMin)
                target.y = _aimData.YBorderMin;
        }
    }
}
