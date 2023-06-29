using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyInvestigateState : EnemyState<RangedEnemy>
{
	NavMeshAgent _agent;

	public float SightDistance => _sightDistance;
	float _sightDistance;
	Vector3 _lastSeenPos;

	public RangedEnemyInvestigateState(RangedEnemy enemy, float sightDistance, LayerMask wall) : base(enemy, wall)
	{
		_sightDistance = sightDistance * sightDistance;
		_agent = enemy.Agent;
	}

	public override void Enter()
	{
		_lastSeenPos = _enemy.Player.transform.position;
	}

	public override void Exit()
	{
	}

	float _patrolTime = 5;
	public override void Update()
	{
		if (_patrolTime > 0)
		{
			_patrolTime -= Time.deltaTime;
			_agent.SetDestination(_lastSeenPos);
		}
		else
		{
			_patrolTime = 5;
			_enemy.SwitchState(_enemy.Patrol);
		}

		if (!Physics.Linecast(_enemy.Eyes.position, _enemy.Player.CameraHolder.position, _wall))
		{
			_patrolTime = 5;
			if (_enemy.SqrDistanceToPlayer <= _enemy.Chase.ChaseDistance) _enemy.SwitchState(_enemy.Chase);
		}
	}
}