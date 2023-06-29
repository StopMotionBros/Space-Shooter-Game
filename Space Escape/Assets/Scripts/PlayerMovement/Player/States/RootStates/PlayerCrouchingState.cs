using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine;

public class PlayerCrouchingState : PlayerState
{
	public override bool RootState => true;

	const float INTERP_DUR = 0.2f;

	Transform _cameraHolder;
	
	float _standHeight;
	float _crouchHeight;

	Vector3 _standCenter;
	Vector3 _crouchCenter;

	Vector3 _standEyePos;
	Vector3 _crouchEyePos;

	CapsuleCollider _collider;

	LayerMask _ground;

	float _radius;
	float _speedMultiplier;

	public PlayerCrouchingState(PlayerController player) : base(player)
	{
		_cameraHolder = _player.CameraHolder;

		_standHeight = _player.StandHeight;
		_crouchHeight = _player.CrouchHeight;
		_speedMultiplier = _player.CrouchMultiplier;

		_collider = _player.Collider;
		_radius = _collider.radius - 0.1f;

		_standCenter = _standHeight * 0.5f * Vector3.up;
		_crouchCenter = _crouchHeight * 0.5f * Vector3.up;

		_standEyePos = (_standHeight - 0.25f) * Vector3.up;
		_crouchEyePos = (_crouchHeight - 0.25f) * Vector3.up;

		_ground = _player.Ground;
	}

	public override void Enter()
	{
		Crouch();

		_player.BodyAnimator.SetBool("Crouching", true);

		Vector3 inputDirection = _player.GetInputDirection();
		if (inputDirection == Vector3.zero) SetSubState(_player.Idle);
		else SetSubState(_player.Walking);

		_player.Actions.Movement.Crouch.canceled += Uncrouch;
	}

	public override void Exit()
	{
		Uncrouch();
		
		_player.BodyAnimator.SetBool("Crouching", false);

		_player.Actions.Movement.Crouch.canceled -= Uncrouch;
	}

	public override void FixedUpdate()
	{
		_player.MovePlayer(false, _speedMultiplier);
	}

	public override void Update()
	{
		if (!_player.OnGround) _player.SwitchState(_player.Falling);
	}

	void Crouch()
	{
		DOVirtual.Float(_collider.height, _crouchHeight, INTERP_DUR, v => _collider.height = v);
		DOVirtual.Vector3(_collider.center, _crouchCenter, INTERP_DUR, v => _collider.center = v);
		_cameraHolder.DOLocalMove(_crouchEyePos, INTERP_DUR);
	}

	void Uncrouch(InputAction.CallbackContext context)
	{
		Vector3 playerPos = _player.transform.position;
		if (!Physics.CheckCapsule(playerPos + (0.3f * Vector3.up), playerPos + ((_standHeight - 0.1f) * Vector3.up), _radius, _ground)) _player.SwitchState(_player.Grounded);
	}

	void Uncrouch()
	{
		DOVirtual.Float(_collider.height, _standHeight, INTERP_DUR, v => _collider.height = v);
		DOVirtual.Vector3(_collider.center, _standCenter, INTERP_DUR, v => _collider.center = v);
		_cameraHolder.DOLocalMove(_standEyePos, INTERP_DUR);
	}
}