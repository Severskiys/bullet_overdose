using System.Threading;
using CodeBase._Tools.StateMachine;
using CodeBase.Infrastructure.States;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure
{
    public class Game : IGameplayStarter, ITickable, IStartable
    {
        private readonly StateMachine _stateMachine;
        private readonly LoadMenuState _loadMenuState;
        private readonly LoadGameplayState _loadGameplayState;

        public Game(IObjectResolver resolver)
        {
            _stateMachine = new StateMachine();
            _loadMenuState = resolver.Resolve<LoadMenuState>();
            _loadGameplayState = resolver.Resolve<LoadGameplayState>();
            GameLoopState gameLoopState = resolver.Resolve<GameLoopState>();
            MenuState menuState = resolver.Resolve<MenuState>();
            _stateMachine.AddTransition(_loadMenuState, menuState, () => _loadMenuState.Complete);
            _stateMachine.AddTransition(_loadGameplayState, gameLoopState, () => _loadGameplayState.Complete);
        }

        public void LoadGameLevel()
        {
            _stateMachine.SetState(_loadGameplayState);
        }

        public void Start()
        {
            _stateMachine.SetState(_loadMenuState);
        }

        public void Tick()
        {
            _stateMachine.Tick();
        }
    }
}
