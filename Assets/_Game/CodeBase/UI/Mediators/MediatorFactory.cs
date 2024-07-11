using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.UI.Mediators
{
    public class MediatorFactory : SaversHolder
    {
        private const string UIRootPath = "UIRoot";
        private readonly AssetProvider _assets;
        private readonly Dictionary<Type, IMediator> _mediators;
        private readonly LifetimeScope _parentScope;
        private Transform _uiRoot;

        public MediatorFactory(AssetProvider assets, LifetimeScope parentScope)
        {
            _parentScope = parentScope;
            _assets = assets;
            _mediators = new Dictionary<Type, IMediator>();
        }

        private async UniTask CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }

        public async UniTask<TMediator> Get<TMediator>() where TMediator : MonoBehaviour, IMediator
        {
            if (_mediators.TryGetValue(typeof(TMediator), out var mediator))
                return mediator as TMediator;
            
            if (_uiRoot == default)
                await CreateUIRoot();

            GameObject mediatorGo = await _assets.Instantiate(typeof(TMediator).Name, _uiRoot);
            TMediator instance = mediatorGo.GetComponent<TMediator>();
            _parentScope.Container.Inject(instance);
            _mediators.Add(instance.GetType(), instance);
            RegisterProgressWatchers(instance.gameObject);
            instance.OnCleanUp += CleanUp;
            return instance;
        }

        private void CleanUp(IMediator mediator)
        {
            mediator.OnCleanUp -= CleanUp;
            if (_mediators.ContainsKey(mediator.GetType()))
                _mediators.Remove(mediator.GetType());
            UnRegisterProgressWatchers(mediator.GameObject);
        }

        public void CleanupAll()
        {
            _uiRoot = default;
            ProgressLoaders.Clear();
            ProgressSavers.Clear();
            _assets.Cleanup();
        }
    }
}
