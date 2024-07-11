using System.Threading.Tasks;
using CodeBase._Services.PersistentGameData;
using CodeBase._Services.StaticData;
using CodeBase._Tools.StateMachine;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.UI.Mediators;

namespace CodeBase.Infrastructure.States
{
    public class LoadGameplayState : ISelfCompleteState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly EcsGameBuilder _ecsGameBuilder;
        private readonly PersistentProgress _progress;
        private readonly StaticDataService _staticData;
        private readonly MediatorFactory _mediatorFactory;

        public bool Complete { get; private set; }

        public LoadGameplayState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, EcsGameBuilder ecsGameBuilder,
            PersistentProgress progress, StaticDataService staticDataService, MediatorFactory mediatorFactory)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _ecsGameBuilder = ecsGameBuilder;
            _progress = progress;
            _staticData = staticDataService;
            _mediatorFactory = mediatorFactory;
        }

        public void OnEnter()
        {
            Complete = false;
            _loadingCurtain.Show();
            _sceneLoader.LoadLevel(OnLoaded);
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
            await _ecsGameBuilder.Build();
            InformProgressLoaders();
            Complete = true;
        }
        
        private void InformProgressLoaders()
        {
            foreach (IProgressLoader progressReader in _ecsGameBuilder.ProgressLoaders)
                progressReader.LoadProgress(_progress.Progress);
        }

        private async Task InitGameWorld()
        {
        }
    }
}