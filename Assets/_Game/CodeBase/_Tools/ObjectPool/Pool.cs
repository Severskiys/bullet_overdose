using System.Collections.Generic;
using UnityEngine;

namespace CodeBase._Tools.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<IPoolable>> _poolItems = new();
        private readonly Dictionary<int, Transform> _containers = new();
        private readonly HashSet<IPoolable> _usedItems = new();
        
        private void OnDestroy()
        {
            _poolItems.Clear();
            _containers.Clear();
            _usedItems.Clear();
        }

        public T Get<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
            where T : MonoBehaviour, IPoolable
        {
            T pooledItem;
            int id = prefab.GetInstanceID();
            Queue<IPoolable> queue = GetQueue(id);
            Transform container = GetContainer(id);

            if (queue.Count > 0)
            {
                pooledItem = (T)queue.Dequeue();
                Transform pooledItemTransform = pooledItem.transform;
                if (parent != default)
                    pooledItemTransform.SetParent(parent, false);

                pooledItemTransform.rotation = rotation;
                pooledItemTransform.position = position;
                pooledItem.gameObject.SetActive(true);
                pooledItem.OnTakeFromPool();
                _usedItems.Add(pooledItem);
                UpdateContainerName(container, queue.Count, prefab.name);
                return pooledItem;
            }

            Transform newParent = parent == default ? container : parent;
            pooledItem = InstantiateObject(prefab, position, rotation, newParent, id);
            pooledItem.OnTakeFromPool();
            UpdateContainerName(container, 0, prefab.name);
            return pooledItem;
        }

        private T InstantiateObject<T>(T prefab, Vector3 position, Quaternion rotation, Transform newParent, int id)
            where T : MonoBehaviour, IPoolable
        {
            Quaternion targetRotation = rotation != default ? rotation : prefab.transform.rotation;
            T instance = Instantiate(prefab, position, targetRotation, newParent);
            instance.name = prefab.name;
            instance.ID = id;
            instance.ContainerName = prefab.name;
            instance.SetReleaseAction(Release);
            _usedItems.Add(instance);
            return instance;
        }

        public void Release<T>(int id, T poolItem) where T : MonoBehaviour, IPoolable
        {
            poolItem.OnReturnToPool();

            Queue<IPoolable> queue = GetQueue(id);
            if (queue.Contains(poolItem) == false)
                queue.Enqueue(poolItem);

            _usedItems.Remove(poolItem);
            Transform container = GetContainer(id);
            poolItem.transform.SetParent(container);
            UpdateContainerName(container, queue.Count, poolItem.ContainerName);
            poolItem.gameObject.SetActive(false);
        }

        private Queue<IPoolable> GetQueue(int id)
        {
            if (_poolItems.TryGetValue(id, out var queue))
                return queue;

            queue = new Queue<IPoolable>();
            _poolItems.Add(id, queue);
            return queue;
        }

        private Transform GetContainer(int id)
        {
            if (_containers.TryGetValue(id, out var container))
                return container;

            container = new GameObject().transform;
            container.parent = transform;
            _containers.Add(id, container);
            return container;
        }

        private void UpdateContainerName(Transform container, int pooled, string name = default)
        {
#if UNITY_EDITOR
            var newName = name ?? container.name;
            if (name != default) container.name = $"{newName}\t{pooled}/{container.childCount}";
#endif
        }
    }
}