using CodeBase.Logic.AimMove;
using UnityEngine;

namespace CodeBase.Logic.UserLogic
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private AimTargetView _aimTargetView;

        public AimTargetView AimTargetView => _aimTargetView;
    }
}
