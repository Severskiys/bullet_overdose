using UnityEngine;

namespace CodeBase.Logic.Road
{
    public class RoadPieceView : MonoBehaviour
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        public Vector3 Forward
        {
            get => transform.forward;
            set => transform.forward = value;
        }
    }
}
