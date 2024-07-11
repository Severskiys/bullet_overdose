using CodeBase._Tools.StateMachine;
using CodeBase.Infrastructure.Factory;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private EcsGameBuilder _builder;

        public GameLoopState(EcsGameBuilder builder)
        {
            _builder = builder;
        }
        
        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            _builder.Cleanup();
        }

        public void Tick()
        {
            _builder.Systems?.Run();
        }
    }
}