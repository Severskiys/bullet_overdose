using System;
using CodeBase.Infrastructure;
using UnityEngine;
using VContainer;

namespace CodeBase.UI.Mediators
{
    public class MenuMediator : MonoBehaviour, IMediator
    {
        public event Action<IMediator> OnCleanUp;

        [SerializeField] private SimpleButton _startGameButton;
        [Inject] private IGameplayStarter _gameplayStarter;
        
        public GameObject GameObject => gameObject;
        
        private void Awake() => _startGameButton.OnClick += SendStartGameSignal;

        private void OnDestroy()
        {
            _startGameButton.OnClick -= SendStartGameSignal;
            OnCleanUp?.Invoke(this);
        }

        private void SendStartGameSignal() => _gameplayStarter.LoadGameLevel();
    }
}