using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    
    [Serializable]
    public class RoadData
    {
        public float BaseSpeed;
        public float FastSpeed;
        public float SlowSpeed;
        public float RoadPieceLength;
        public int PieceCount;
    }
}
