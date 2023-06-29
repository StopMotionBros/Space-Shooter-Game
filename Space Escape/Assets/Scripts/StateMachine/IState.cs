using System.Collections;

public interface IState
{
	public void Enter();
	public void Update();
	public void Exit();
	public void Destroy();
}