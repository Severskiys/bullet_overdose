using CodeBase._Tools.StateMachine;

namespace CodeBase.Infrastructure.States
{
    public class LoadGameDataState : ISelfCompleteState
    {
        public bool Complete { get; private set; }
        
        public LoadGameDataState()
        {
            // прогреем ассет менеджер
            // загрузим статик дату
            // загрузим сейвы игрока
        }
        
        public void OnEnter()
        {
            Complete = false;
        }

        public void OnExit()
        {

        }

        public void Tick()
        {
            
        }

        private async void LoadData()
        {
            Complete = true;
        }
        
    }
}
