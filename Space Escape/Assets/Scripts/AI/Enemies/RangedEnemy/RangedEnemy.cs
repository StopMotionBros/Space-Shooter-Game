using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class RangedEnemy : Enemy
{
	[SerializeField] float _sightDistance;
	[SerializeField] float _chaseDistance;
	[SerializeField] float _attackDistance;
	[SerializeField] float _takeCoverDistance;

	public float SightAngle => _sightAngle;
	[SerializeField] float _sightAngle;

	[SerializeField] float _fov;
	[SerializeField] float _sightRatio;

	[SerializeField] float _investigateSpeed;
	[SerializeField] float _chaseSpeed;

	[SerializeField] LayerMask _wall;

	public RangedEnemyPatrolState Patrol;
	public RangedEnemyGuardState Guard;
	public RangedEnemyInvestigateState Investigate;
	public RangedEnemyChaseState Chase;
	public RangedEnemyAttackState Attack;

	void Awake()
	{
		Patrol = new RangedEnemyPatrolState(this, FindObjectsOfType<AIPath>(), _wall);
		Guard = new RangedEnemyGuardState(this, _wall);
		Investigate = new RangedEnemyInvestigateState(this, _sightDistance, _wall);
		Chase = new RangedEnemyChaseState(this, _chaseDistance, _wall);
		Attack = new RangedEnemyAttackState(this, _wall, _attackDistance, _takeCoverDistance);

		SwitchState(Patrol);
	}

	void Update()
	{
		CurrentState.Update();
	}

	void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position + (0.5f * _agent.height * Vector3.up);

		Quaternion posRot = Quaternion.Euler(0, _sightAngle, 0);
		Quaternion negRot = Quaternion.Euler(0, -_sightAngle, 0);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(pos, pos + (posRot * (_sightDistance * transform.forward)));
		Gizmos.DrawLine(pos, pos + (negRot * (_sightDistance * transform.forward)));

		Gizmos.color = Color.red;
		Gizmos.DrawLine(pos, pos + (posRot * (_chaseDistance * transform.forward)));
		Gizmos.DrawLine(pos, pos + (negRot * (_chaseDistance * transform.forward)));
	}
}