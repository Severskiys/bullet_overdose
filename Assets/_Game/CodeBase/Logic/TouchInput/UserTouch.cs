using UnityEngine;

namespace CodeBase.Logic.TouchInput
{
    public struct UserTouch
    {
        public bool IsOff;
        public float StartTime;
        public Vector2 StartPosition;
        public float CurrentTime;
        public Vector2 CurrentPosition;
        public Vector2 Delta;
        public Vector2 Direction;
        public float SqrDist;
        public bool HadStartPhase;
        public float Duration;
        public Vector2 SwipeDelta;
        public TouchPhase CurrentPhase;
    }
}
