using UnityEngine;

public class PlayerDeadState : PlayerState
{
	public override bool RootState => true;

	public PlayerDeadState(PlayerController player) : base(player)
	{
	}

	public override void Enter()
	{
		Debug.Log("I have deathdified.");
	}

	public override void Exit()
	{
	}

	public override void FixedUpdate()
	{
	}

	public override void Update()
	{
	}
}
