using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
	public override bool RootState => true;

	Rigidbody _rigidbody;
	float _groundDrag;

	Animator _bodyAnimator;

	public PlayerGroundedState(PlayerController player) : base(player)
	{
		_rigidbody = _player.Rigidbody;
		_groundDrag = _player.GroundDrag;
		_bodyAnimator = _player.BodyAnimator;
	}

	public override void Enter()
	{
		if (_player.Actions.Movement.Jump.IsPressed())
		{
			Jump(default);
			return;
		}

		_bodyAnimator.SetBool("Grounded", true);

		Vector3 inputDirection = _player.GetInputDirection();
		if (inputDirection == Vector3.zero) SetSubState(_player.Idle);
		else if (inputDirection.z > 0 && _player.Actions.Movement.Sprint.IsPressed()) SetSubState(_player.Sprinting);
		else SetSubState(_player.Walking);

		_rigidbody.drag = _groundDrag;
		_player.Actions.Movement.Jump.started += Jump;
		_player.Actions.Movement.Crouch.started += Crouch;
	}

	public override void Exit()
	{
		_player.Actions.Movement.Jump.started -= Jump;
		_player.Actions.Movement.Crouch.started -= Crouch;
	}

	public override void FixedUpdate()
	{
		_player.MovePlayer();
		if (!_player.OnGround) _player.SwitchState(_player.Falling);
	}

	public override void Update()
	{
	}

	void Jump(InputAction.CallbackContext context) => _player.SwitchState(_player.Jumping);
	void Crouch(InputAction.CallbackContext context) => _player.SwitchState(_player.Crouching);
}