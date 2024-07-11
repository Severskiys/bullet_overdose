using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Mediators
{
    public class SimpleButton : MonoBehaviour
    {
        public event Action OnClick;
        
        [SerializeField] private Button _button;
        
     // [TypeFilter(typeof(IMediator))] [SerializeField] private SerializableType _type;
     
        private void Awake()
        {
            _button.onClick.AddListener(SendClickEvent);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(SendClickEvent);
        }

        private void SendClickEvent()
        {
            OnClick?.Invoke();
        }
    }
}