using System;
using UnityEngine;

namespace CodeBase._Tools.ObjectPool
{
    [SelectionBase]
    public abstract class PoolItem : MonoBehaviour, IPoolable
    {
        public int ID { get; set; }
        public string ContainerName { get; set; }

        protected Action<int, PoolItem> _releaseAction;
        public void SetReleaseAction(Action<int, PoolItem> releaseAction) => _releaseAction = releaseAction;

        public virtual void OnTakeFromPool()
        {
        }

        public virtual void OnReturnToPool()
        {
        }
    }
}