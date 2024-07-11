using System;
using UnityEngine;

namespace CodeBase._Tools
{
    [Serializable]
    public struct ReactiveVariable<T>
    {
        private event Action<T> OnChange;

        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChange?.Invoke(value);
            }
        }
        
        public ReactiveVariable(T value)
        {
            _value = value;
            OnChange = default;
        }
        
        public void Subscribe(Action<T> action)
        {
            OnChange += action;
        }

        public void Unsubscribe(Action<T> action)
        {
            OnChange -= action;
        }
    }
}