using UnityEngine;

public class Medkit : UsableItem
{
	[SerializeField] int _healAmount;
	Health _health;

	protected override void Init()
	{
		_health = _player.GetComponentInChildren<Health>();
	}

	public override void Use()
	{
		_health.Heal(_healAmount);
	}
}
