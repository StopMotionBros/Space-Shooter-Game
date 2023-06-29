using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class RangedEnemyAttackState : EnemyState<RangedEnemy>
{
	public float AttackDistance => _attackDistance;
	float _attackDistance;

	public float TakeCoverDistance => _takeCoverDistance;
	float _takeCoverDistance;

	NavMeshAgent _agent;

	public RangedEnemyAttackState(RangedEnemy enemy, LayerMask wall, float attackDistance, float takeCoverDistance) : 
		base(enemy, wall)
	{
		_attackDistance = attackDistance * attackDistance;
		_takeCoverDistance = takeCoverDistance * takeCoverDistance;

		_agent = enemy.Agent;
	}

	public override void Enter()
	{
		_agent.SetDestination(_enemy.transform.position);
	}

	public override void Exit()
	{
	}

	float _attackTime = 3;
	public override void Update()
	{
		float playerDist = _enemy.SqrDistanceToPlayer;
		Vector3 directionToPlayer = _enemy.DirectionToPlayer;

		if (_attackTime <= 0)
		{
			_attackTime = 3;
			if (Physics.Raycast(_enemy.Eyes.position, _enemy.Eyes.forward, out RaycastHit hit, 150, 1 << 3))
			{
				if (hit.collider.TryGetComponent(out Health health))
				{
					health.TakeDamage(10);
				}
			}
		}
		else _attackTime -= Time.deltaTime;

		if (playerDist >= _takeCoverDistance)
		{
			if (FindCover(_enemy.transform.position, out Transform cover))
			{
				_agent.SetDestination(cover.position - directionToPlayer);
			}
		}
		else
		{
			_agent.SetDestination(_enemy.transform.position - (_agent.stoppingDistance * 2 * directionToPlayer));
			if (playerDist <= _agent.stoppingDistance * _agent.stoppingDistance)
			{
				_enemy.Player.Rigidbody.AddForce(20 * directionToPlayer, ForceMode.VelocityChange);
			}
		}

		Vector3 rotToPlayer = Quaternion.LookRotation(_enemy.DirectionToPlayerEyes).eulerAngles;
		_enemy.transform.localEulerAngles = new Vector3(0, rotToPlayer.y, 0);
		_enemy.Eyes.transform.localEulerAngles = new Vector3(rotToPlayer.x, 0, 0);

		if (playerDist > _attackDistance) _enemy.SwitchState(_enemy.Chase);
	}

	bool FindCover(Vector3 position, out Transform closestCover)
	{
		closestCover = null;
		Collider[] covers = Physics.OverlapSphere(position, _attackDistance, 1 << 8);
		float sqrDistance = float.MaxValue;

		foreach (Collider col in covers)
		{
			float distance = (position - col.transform.position).sqrMagnitude;
			if (distance < sqrDistance)
			{
				sqrDistance = distance;
				closestCover = col.transform;
			}
		}

		if ((closestCover.position - _enemy.Player.transform.position).sqrMagnitude > _attackDistance) return false;

		return closestCover != null;
	}
}
