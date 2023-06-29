using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerState
{
	public override bool RootState => false;

	float _speed;

	Transform _mantleCheck;
	float _mantleCheckDistance;

	LayerMask _ground;

	Vector3 _currentInput;

	Animator _bodyAnimator;

	public PlayerSprintingState(PlayerController player) : base(player)
	{
		_speed = _player.SprintSpeed;

		_mantleCheck = _player.MantleCheck;
		_mantleCheckDistance = _mantleCheck.localPosition.y - 0.2f;

		_bodyAnimator = _player.BodyAnimator;

		_ground = _player.Ground;
	}

	public override void Enter()
	{
		_player.BodyAnimator.SetBool("Sprinting", true);

		_player.SetSpeed(_speed);
		_player.Actions.Movement.Sprint.canceled += StopSprint;
	}

	public override void Exit()
	{
		_player.BodyAnimator.SetBool("Sprinting", false);

		_player.Actions.Movement.Sprint.canceled -= StopSprint;
	}

	public override void FixedUpdate()
	{
	}

	public override void Update()
	{
		Vector3 input = _player.GetInputDirection();
		_currentInput = Vector3.Lerp(_currentInput, input, 5 * Time.deltaTime);
		_bodyAnimator.SetFloat("X", _currentInput.x);

		if (input.z <= 0) SwitchState(_player.Walking);
		else if (Physics.Raycast(_mantleCheck.position, Vector3.down, out RaycastHit hit, _mantleCheckDistance, _ground))
		{
			_player.Mantle.SetHit(hit);
			SwitchState(_player.Mantle);
		}
	}

	void StopSprint(InputAction.CallbackContext context) => SwitchState(_player.Walking);
}
