using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class RangedEnemyPatrolState : EnemyState<RangedEnemy>
{
	NavMeshAgent _agent;

	AIPath[] _paths;
	AIPath _currentPath;
	AIWaypoint _currentWaypoint;

	int _direction;

	public RangedEnemyPatrolState(RangedEnemy enemy, AIPath[] paths, LayerMask wall) : base(enemy, wall)
	{
		_agent = _enemy.Agent;
		_paths = paths;
	}

	public override void Enter()
	{
		_currentPath = GetClosestPath(_enemy.transform.position, out AIWaypoint waypoint);
		_currentWaypoint = waypoint;
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
		Vector3 waypointPos = _currentWaypoint.GetPosition();

		float sqrDistance = (_enemy.transform.position - waypointPos).sqrMagnitude;
		if (sqrDistance <= _agent.stoppingDistance * _agent.stoppingDistance) _currentWaypoint = GetNextPosition();

		_agent.SetDestination(waypointPos);

		if (!Physics.Linecast(_enemy.Eyes.position, _enemy.Player.CameraHolder.position, _wall))
		{
			if (_enemy.AngleToPlayer <= _enemy.SightAngle)
				if (_enemy.SqrDistanceToPlayer <= _enemy.Investigate.SightDistance) _enemy.SwitchState(_enemy.Investigate);
		}
	}

	AIWaypoint GetNextPosition()
	{
		AIWaypoint branch = _currentWaypoint.Branch();
		if (branch != _currentWaypoint)
		{
			_direction = Random.Range(0, 2) == 1 ? 1 : -1;
			return branch;
		}

		AIWaypoint next;
		if(_direction >= 0)
		{
			if (_currentWaypoint.Next)
			{
				next = _currentWaypoint.Next;
			}
			else
			{
				next = _currentWaypoint.Previous;
				_direction = -1;
			}
		}
		else
		{
			if (_currentWaypoint.Previous)
			{
				next = _currentWaypoint.Previous;
			}
			else
			{
				next = _currentWaypoint.Next;
				_direction = 1;
			}
		}

		return next;
	}

	AIPath GetClosestPath(Vector3 position, out AIWaypoint closestWaypoint)
	{
		float sqrDist = float.MaxValue;
		AIPath closestPath = _paths[0];
		closestWaypoint = closestPath[0];

		foreach (AIPath path in _paths)
		{
			foreach (AIWaypoint waypoint in path.Waypoints)
			{
				float newDist = (waypoint.transform.position + path.transform.position - position).sqrMagnitude;
				if (newDist < sqrDist)
				{
					sqrDist = newDist;
					closestWaypoint = waypoint;
					if (closestPath != path) closestPath = path;
				}
			}
		}

		return closestPath;
	}
}