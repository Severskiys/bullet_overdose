using CodeBase.Logic.AimMove;
using CodeBase.Logic.HealthLogick;
using CodeBase.Logic.Road;
using CodeBase.Logic.TouchInput;
using CodeBase.Logic.UserLogic;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Logic.Ecs
{
    public sealed class GameAspect : ProtoAspectInject
    {
        public readonly ProtoPool<UserTouch> Touchs;
        public readonly ProtoIt TouchsIt = new (It.Inc<UserTouch>());        
        
        public readonly ProtoPool<RoadPiece> RoadPieces;
        public readonly ProtoIt RoadPieceIt = new (It.Inc<RoadPiece>());
        
        public readonly ProtoPool<Player> PlayerPool;
        public readonly ProtoIt PlayerIt = new (It.Inc<Player>());
        
        public readonly ProtoPool<AimTarget> AimTargets;
        public readonly ProtoIt AimTargetIt = new (It.Inc<AimTarget>());
        
        public readonly ProtoPool<Health> Healths;
        public readonly ProtoIt HealthsIt = new (It.Inc<Health>());
    }
}
