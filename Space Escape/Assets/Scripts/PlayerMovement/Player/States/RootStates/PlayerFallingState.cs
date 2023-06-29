using UnityEngine;

public class PlayerFallingState : PlayerState
{
	public override bool RootState => true;

	float _airMultiplier;

	public PlayerFallingState(PlayerController player) : base(player)
	{
		_airMultiplier = _player.AirMultiplier;
	}

	public override void Enter()
	{
		_player.BodyAnimator.SetBool("Grounded", false);

		_player.Rigidbody.drag = 0;
		_player.Rigidbody.useGravity = true;

		Vector3 inputDirection = _player.GetInputDirection();
		if (inputDirection == Vector3.zero) SetSubState(_player.Idle);
		else if (inputDirection.z > 0 && _player.Actions.Movement.Sprint.IsPressed()) SetSubState(_player.Sprinting);
		else SetSubState(_player.Walking);
	}

	public override void Exit()
	{
	}

	public override void FixedUpdate()
	{
		_player.MovePlayer(true, _airMultiplier);
	}

	public override void Update()
	{
		if(_player.OnGround)
		{
			_player.SwitchState(_player.Grounded);
		}
	}
}