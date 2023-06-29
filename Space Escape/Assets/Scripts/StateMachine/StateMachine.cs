using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : IState
{
	public T CurrentState { get; private set; }

	public void SwitchState(T state)
	{
		if (CurrentState != null) CurrentState.Exit();
		
		CurrentState = state;
		CurrentState.Enter();
	}

	void OnDestroy()
	{
		if (CurrentState != null) CurrentState.Destroy();
	}
}