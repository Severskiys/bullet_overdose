using System;
using UnityEngine;

namespace CodeBase.Logic.AimMove
{
    public class AimTargetView : MonoBehaviour
    {
        private Transform _transform;
        private void Awake() => _transform = transform;

        public Vector3 LocalPosition
        {
            get =>  _transform.localPosition;
            set => _transform.localPosition = value;
        }
    }
}
