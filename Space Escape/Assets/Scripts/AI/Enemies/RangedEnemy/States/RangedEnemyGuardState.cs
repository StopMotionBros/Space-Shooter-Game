using UnityEngine.AI;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RangedEnemyGuardState : EnemyState<RangedEnemy>
{
	NavMeshAgent _agent;

	Vector3 _guardPos;
	Quaternion _guardRot;

	bool _atGuardSpot;

	public RangedEnemyGuardState(RangedEnemy enemy, LayerMask wall) : base(enemy, wall)
	{
		_agent = _enemy.Agent;

		_guardPos = _enemy.transform.position;
		_guardRot = _enemy.transform.rotation;
	}

	public override void Enter()
	{
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
		float sqrDistance = (_enemy.transform.position - _guardPos).sqrMagnitude;

		if (_atGuardSpot)
		{
			if (sqrDistance > _agent.stoppingDistance * _agent.stoppingDistance)
				_atGuardSpot = false;
		}
		else
		{
			_agent.SetDestination(_guardPos);
			if (sqrDistance <= _agent.stoppingDistance * _agent.stoppingDistance)
			{
				_enemy.transform.DORotate(_guardRot.eulerAngles, 0.5f);
				_atGuardSpot = true;
			}
		}

		if (!Physics.Linecast(_enemy.Eyes.position, _enemy.Player.CameraHolder.position, _wall))
		{
			if (_enemy.AngleToPlayer <= _enemy.SightAngle)
				if (_enemy.SqrDistanceToPlayer <= _enemy.Investigate.SightDistance) _enemy.SwitchState(_enemy.Investigate);
		}
	}
}