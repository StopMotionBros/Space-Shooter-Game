using DG.Tweening;
using UnityEngine;

public class PlayerMantleState : PlayerState
{
	public override bool RootState => false;

	Transform _mantleCheck;
	Rigidbody _rigidBody;

	RaycastHit _hit;

	public PlayerMantleState(PlayerController player) : base(player)
	{
		_mantleCheck = _player.MantleCheck;
		_rigidBody = _player.Rigidbody;
	}

	public override void Enter()
	{
		_rigidBody.isKinematic = true;
		Vector3 velocity = _rigidBody.velocity;
		_player.transform.DOMove(_hit.point, 0.2f).OnComplete(() => 
		{
			SwitchState(_player.Sprinting);
			_rigidBody.isKinematic = false;
			_rigidBody.velocity = velocity;
		}).SetEase(Ease.InSine);
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

	public void SetHit(RaycastHit hit) => _hit = hit;
}
