using System.Collections.Generic;
using System.Linq;
using CodeBase._Services.StaticData;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Ecs;
using CodeBase.StaticData;
using Cysharp.Threading.Tasks;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CodeBase.Logic.Road
{
    public class RoadRunSystem : IProtoRunSystem, IProtoInitSystem
    {
        private readonly List<RoadPieceView> _pieces = new List<RoadPieceView>();
        private readonly RoadData _roadData;
        private readonly Transform _roadParent;
        private Vector3 _moveDirection = Vector3.zero;
        private float _speed;
        private GameAspect _aspect;
        private GameFactory _gameFactory;

        public RoadRunSystem(StaticDataService staticData, Level level, GameAspect aspect, GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _aspect = aspect;
            _roadParent = level.RoadParent;
            _roadData = staticData.GetRoadData();
        }

        public void Run()
        {
            foreach (var entity in _aspect.RoadPieceIt)
            {
               ref var roadPieceComponent = ref _aspect.RoadPieces.Get(entity);
               roadPieceComponent.View.Position += _speed * _moveDirection;
               
            }
            
            TryRelocatePiece();
        }
        
        private void TryRelocatePiece()
        {
            RoadPieceView roadPiece = _pieces.Last();
            if (roadPiece.transform.position.z > 110)
            {
                _pieces.Remove(roadPiece);
                Vector3 newForward =  _pieces.First().transform.forward + GetAdditionalRotation();
                roadPiece.transform.forward = newForward;
                roadPiece.transform.position = _pieces.First().transform.position +
                                               -roadPiece.transform.forward * _roadData.RoadPieceLength;
                _pieces.Insert(0, roadPiece);

                RoadPieceView current = _pieces[^2];
                _moveDirection = current.transform.forward;
                RotateCameraAsync();
            }
        }

        private async void RotateCameraAsync()
        {
            Vector3 startForward = Camera.main.transform.forward;
            Vector3 endForward = -_moveDirection;
            float timer = 1.0f;
            float percent = 0f;

            while (percent < 1)
            {
                percent += Time.deltaTime / timer;
                Camera.main.transform.forward = Vector3.Lerp(startForward, endForward, percent);
                await UniTask.Yield();
            }
        }

        private static Vector3 GetAdditionalRotation()
        {
            return Random.Range(0, 5) < 1 ? new Vector3(Random.Range(-0.2f, 0.2f), 0, 0) : Vector3.zero;
        }

        public void Init(IProtoSystems systems)
        {
            for (int i = 0; i < _roadData.PieceCount; i++)
            {
                ref RoadPiece roadEntity = ref _aspect.RoadPieces.NewEntity();
                roadEntity.View = _gameFactory.GetRoadPiece(i, _roadData.RoadPieceLength);
                _pieces.Add(roadEntity.View);
            }

            _speed = _roadData.BaseSpeed;
            _moveDirection = _roadParent.forward;
        }
    }
}
