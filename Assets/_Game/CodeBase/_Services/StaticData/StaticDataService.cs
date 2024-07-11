using CodeBase.Logic.AimMove;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase._Services.StaticData
{
    public class StaticDataService : IService
    {
        private const string GameDataPath = "StaticData/GameData";
        private readonly GameData _gameData;
        private RoadData _roadData;
        private PlayerData _playerData;
        private int _enemiesCount;
        
        public StaticDataService()
        {
            _gameData = Resources.Load<GameData>(GameDataPath);
        }

        public PlayerData GetPlayerData() => _gameData.PlayerData;
        public RoadData GetRoadData() => _gameData.RoadData;
        public AimData GetAimData() => _gameData.AimData;
    }
}
