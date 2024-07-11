using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase._Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCashe = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public AssetProvider()
        {
            Addressables.InitializeAsync();
        }

        public Object RoadPiecePrefab { get; set; }

        public async UniTask<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCashe.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), cacheKey: assetReference.AssetGUID);
        }

        public async UniTask<T> Load<T>(string address) where T : class
        {
            if (_completedCashe.TryGetValue(address, out AsyncOperationHandle completedHandle)) 
                return completedHandle.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return await RunWithCacheOnComplete(handle, address);
        }

        public UniTask<GameObject> Instantiate(string address, Vector3 at) => Addressables.InstantiateAsync(address, at, Quaternion.identity).ToUniTask();
        public UniTask<GameObject> Instantiate(string address) => Addressables.InstantiateAsync(address).ToUniTask();
        public UniTask<GameObject> Instantiate(string address, Transform parent) => Addressables.InstantiateAsync(address, parent).ToUniTask();

        public void Cleanup()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
            foreach (AsyncOperationHandle handle in resourceHandles)
                Addressables.Release(handle);

            _completedCashe.Clear();
            _handles.Clear();
        }

        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle => { _completedCashe[cacheKey] = completeHandle; };
            AddHandle<T>(cacheKey, handle);
            return await handle.ToUniTask();
        }

        private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        public async Task LoadSceneSingle(string sceneName) => await Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}