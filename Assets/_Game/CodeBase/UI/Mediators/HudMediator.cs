using System;
using CodeBase.Infrastructure;
using UnityEngine;
using VContainer;

namespace CodeBase.UI.Mediators
{
    public class HudMediator : MonoBehaviour, IMediator
    {
        public event Action<IMediator> OnCleanUp;
        [SerializeField] private SimpleButton _startGameButton;
        private IGameplayStarter _gameplayStarter;
        public GameObject GameObject => gameObject;

        [Inject]
        public void Construct(IGameplayStarter gameplayStarter)
        {
            _gameplayStarter = gameplayStarter;
        }

        private void Awake()
        {
            _startGameButton.OnClick += SendStartGameSignal;
        }

        private void OnDestroy()
        {
            _startGameButton.OnClick -= SendStartGameSignal;
            OnCleanUp?.Invoke(this);
        }

        private void SendStartGameSignal()
        {
            _gameplayStarter.LoadGameLevel();
        }
    }
}
