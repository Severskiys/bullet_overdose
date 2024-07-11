using UnityEngine;

namespace CodeBase._Tools.ObjectPool
{
    [SelectionBase]
    public abstract class ReleaseAblePoolItem : PoolItem
    {
        public void Release() => _releaseAction.Invoke(ID, this);
    }
}