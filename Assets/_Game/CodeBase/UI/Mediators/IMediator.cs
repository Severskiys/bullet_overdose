using System;
using UnityEngine;

namespace CodeBase.UI.Mediators
{
    public interface IMediator
    {
        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject { get; }
    }
}