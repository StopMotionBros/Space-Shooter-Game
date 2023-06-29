public abstract class PlayerState : IState
{
	public abstract bool RootState { get; }

	protected PlayerController _player;

	PlayerState _superState;
	PlayerState _subState;

	protected PlayerState(PlayerController player)
	{
		_player = player;
	}

	public abstract void Enter();
	public abstract void Exit();
	public abstract void Update();
	public abstract void FixedUpdate();

	public void UpdateStates()
	{
		Update();
		if (_subState != null) _subState.Update();
	}

	protected void SwitchState(PlayerState state)
	{
		if (RootState) _player.SwitchState(state);
		else _superState.SetSubState(state);
	}

	protected void SetSuperState(PlayerState state)
	{
		_superState = state;
	}

	protected void SetSubState(PlayerState state)
	{
		if (_subState != null) _subState.Exit();

		_subState = state;
		_subState.SetSuperState(this);

		_subState.Enter();
	}

	public virtual void Destroy() { }
}