using System.Collections;
using UnityEngine;

public abstract class EnemyState<T> : IState where T : Enemy
{
	protected T _enemy;
	protected LayerMask _wall;

	public EnemyState(T enemy, LayerMask wall)
	{
		_enemy = enemy;
		_wall = wall;
	}

	public abstract void Enter();
	public abstract void Exit();
	public abstract void Update();

	public virtual void Destroy() { }
}