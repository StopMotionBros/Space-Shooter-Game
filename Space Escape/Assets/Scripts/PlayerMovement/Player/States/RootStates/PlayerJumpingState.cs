using UnityEngine;

public class PlayerJumpingState : PlayerState
{
	public override bool RootState => true;

	float _jumpForce;
	bool _jump;

	float _airMultiplier;

	public PlayerJumpingState(PlayerController player) : base(player)
	{
		_jumpForce = _player.JumpForce;
		_airMultiplier = _player.AirMultiplier;
	}

	public override void Enter()
	{
		_player.BodyAnimator.SetBool("Grounded", false);

		_jump = true;
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

		if (!_jump) return;

		Vector3 vel = _player.Rigidbody.velocity;
		_player.Rigidbody.velocity = vel - (vel.y * Vector3.up);
		_player.Rigidbody.AddForce(_jumpForce * Vector3.up, ForceMode.Impulse);

		_jump = false;
	}

	public override void Update()
	{
		if (_player.Rigidbody.velocity.y < 0) _player.SwitchState(_player.Falling);
	}
}