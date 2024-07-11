using System;
using CodeBase.UI.Mediators;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private void Awake() => _button.onClick.AddListener(Open);
        private void Open()
        {
          
        }

        public void Init(MediatorFactory mediatorFactory)
        {
           
        }
    }
}