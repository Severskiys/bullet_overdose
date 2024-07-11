using UnityEngine;

namespace CodeBase.Logic
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _roadParent;
        [SerializeField] private Transform _playerSpawnPoint;

        public Transform RoadParent => _roadParent;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
    }
}
