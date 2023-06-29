using UnityEngine.InputSystem;
using UnityEngine;
using DG.Tweening;

public class PlayerWalkingState : PlayerState
{
	public override bool RootState => false;

	float _speed;

	Animator _bodyAnimator;

	Vector3 _currentInput;

	public PlayerWalkingState(PlayerController player) : base(player)
	{
		_speed = _player.WalkSpeed;
		_bodyAnimator = _player.BodyAnimator;
	}

	public override void Enter()
	{
		_player.SetSpeed(_speed);

		_player.Actions.Movement.Sprint.started += Sprint;
	}

	public override void Exit()
	{
		_player.Actions.Movement.Sprint.started -= Sprint;
	}

	public override void FixedUpdate()
	{
	}

	public override void Update()
	{
		Vector3 input = _player.GetInputDirection();
		_currentInput = Vector3.Lerp(_currentInput, input, 5 * Time.deltaTime);
		_bodyAnimator.SetFloat("X", _currentInput.x);
		_bodyAnimator.SetFloat("Y", _currentInput.z);

		if (input == Vector3.zero)
		{
			DOVirtual.Float(_currentInput.x, 0, 0.2f, v => _bodyAnimator.SetFloat("X", v));
			DOVirtual.Float(_currentInput.z, 0, 0.2f, v => _bodyAnimator.SetFloat("Y", v));
			_currentInput = Vector3.zero;
			SwitchState(_player.Idle);
		}
	}

	void Sprint(InputAction.CallbackContext context)
	{
		if (_player.GetInputDirection().z > 0 && _player.CurrentState != _player.Crouching) SwitchState(_player.Sprinting);
	}
}
