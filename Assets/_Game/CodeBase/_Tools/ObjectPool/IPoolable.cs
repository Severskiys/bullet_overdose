using System;

namespace CodeBase._Tools.ObjectPool
{
    public interface IPoolable
    {
        public int ID { get; set; }
        public string ContainerName { get; set;}
        public void SetReleaseAction(Action<int, PoolItem> releaseAction);
        public void OnReturnToPool();
        public void OnTakeFromPool();
    }
}