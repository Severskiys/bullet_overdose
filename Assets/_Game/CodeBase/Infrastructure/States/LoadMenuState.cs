using CodeBase._Services.PersistentGameData;
using CodeBase._Services.StaticData;
using CodeBase._Tools.StateMachine;
using CodeBase.Logic;
using CodeBase.UI.Mediators;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class LoadMenuState : ISelfCompleteState
    {
        public bool Complete { get; private set; }
        
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly PersistentProgress _progress;
        private readonly StaticDataService _staticData;
        private readonly MediatorFactory _mediatorFactory;

        public LoadMenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, PersistentProgress progress,
            StaticDataService staticDataService, MediatorFactory mediatorFactory)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _progress = progress;
            _staticData = staticDataService;
            _mediatorFactory = mediatorFactory;
        }
        
        public void OnEnter()
        {
            Complete = false;
            _sceneLoader.LoadMenu(OnLoaded);
        }

        public void OnExit()
        {
            _loadingCurtain.Hide();
        }

        public void Tick()
        {
            
        }

        private async void OnLoaded()
        {
            await _mediatorFactory.Get<MenuMediator>();
            InformProgressLoaders();
            Complete = true;
        }

        private void InformProgressLoaders()
        {
            foreach (IProgressLoader loader in _mediatorFactory.ProgressLoaders)
                loader.LoadProgress(_progress.Progress);
        }

        private async UniTask InitGameWorld()
        {
           await UniTask.Yield();
        }
    }
}