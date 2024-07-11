using UnityEngine;

namespace CodeBase.Logic.AimMove
{
    public class AimView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public Vector2 AnchoredPosition
        {
            get =>  _rectTransform.anchoredPosition;
            set => _rectTransform.anchoredPosition = value;
        }
    }
}
