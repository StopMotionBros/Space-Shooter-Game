using System.Collections;
using UnityEngine;

public class RangedEnemyChaseState : EnemyState<RangedEnemy>
{
	public float ChaseDistance => _chaseDistance;
	float _chaseDistance;

	public RangedEnemyChaseState(RangedEnemy enemy, float chaseDistance, LayerMask _wall) : base(enemy, _wall)
	{
		_chaseDistance = chaseDistance * chaseDistance;
	}

	public override void Enter()
	{
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
		_enemy.FollowPlayer();

		if (_enemy.SqrDistanceToPlayer <= _enemy.Attack.AttackDistance) _enemy.SwitchState(_enemy.Attack);

		if (Physics.Linecast(_enemy.Eyes.position, _enemy.Player.CameraHolder.position, _wall)
			|| _enemy.SqrDistanceToPlayer > _chaseDistance)
		{
			_enemy.SwitchState(_enemy.Investigate);
		}
	}
}
