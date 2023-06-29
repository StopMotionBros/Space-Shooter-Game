using UnityEngine;

public class PlayerIdleState : PlayerState
{
	public override bool RootState => false;

	float _speed;

	public PlayerIdleState(PlayerController player) : base(player)
	{
		_speed = 0;
	}

	public override void Enter()
	{
		_player.SetSpeed(_speed);
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
		if (_player.GetInputDirection() != Vector3.zero) SwitchState(_player.Walking);
	}

	public override void FixedUpdate()
	{
		_player.MovePlayer();
	}
}