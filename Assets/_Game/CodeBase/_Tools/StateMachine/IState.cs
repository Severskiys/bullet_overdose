namespace CodeBase._Tools.StateMachine
{
	public interface IState
	{
		void OnEnter();
		void OnExit();
		void Tick();
	}

	public interface ISelfCompleteState : IState
	{
		public bool Complete { get; }
	}
}
