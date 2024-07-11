using System;
using CodeBase.Infrastructure.AssetManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly AssetProvider _assetProvider;

        public SceneLoader(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async void LoadMenu(Action onLoaded = null)
        {
           await _assetProvider.LoadSceneSingle("MenuScene");
            onLoaded?.Invoke();
        }

        public async void LoadLevel(Action onLoaded = null)
        {
            await _assetProvider.LoadSceneSingle($"Level");
            onLoaded?.Invoke();
        }
    }
}
