using System;
using UnityEngine;

namespace CodeBase.Logic.AimMove
{
    [Serializable]
    public class AimData
    {
        public float XBorderMax, XBorderMin, YBorderMax, YBorderMin;
        public Vector3 StartLocalPos;
        public float DefaultSpeed;
        public float XSpeedModifier;
        public float YSpeedModifier;
    }
}
