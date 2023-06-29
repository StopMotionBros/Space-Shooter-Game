using UnityEngine.AI;
using UnityEngine;

public abstract class Enemy : StateMachine<IState>
{
    public NavMeshAgent Agent => _agent;
    [SerializeField] protected NavMeshAgent _agent;

    public PlayerController Player => _player;
    [SerializeField] protected PlayerController _player;

    public Transform Eyes => _eyes;
    [SerializeField] Transform _eyes;

    public float SqrDistanceToPlayer => (transform.position - _player.transform.position).sqrMagnitude;
    public float AngleToPlayer => Vector3.Angle((_player.transform.position - transform.position).normalized, transform.forward);
    public Vector3 DirectionToPlayer => (_player.transform.position - transform.position).normalized;
    public Vector3 DirectionToPlayerEyes => (_player.CameraHolder.transform.position - Eyes.position).normalized;
    
	public void FollowPlayer() => _agent.SetDestination(_player.transform.position);
}
