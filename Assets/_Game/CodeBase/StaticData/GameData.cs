using CodeBase.Logic.AimMove;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "GameData", menuName = "StaticData/GameData")]
    public class GameData : ScriptableObject
    {
        public PlayerData PlayerData;
        public AimData AimData;
        public RoadData RoadData;
        public int EnemiesCount;
    }
}