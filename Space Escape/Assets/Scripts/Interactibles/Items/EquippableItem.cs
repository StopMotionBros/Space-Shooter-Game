using UnityEngine.InputSystem;
using UnityEngine;

public abstract class EquippableItem : Item
{
	[SerializeField] protected Animator _animator;
	[SerializeField] protected Animator _armsAnimator;

	public bool Equipped { get; private set; }

	protected override void Init()
	{
		_armsAnimator = _player.ArmsAnimator;
		Unequip();
	}

	public override void Use()
	{
		gameObject.SetActive(true);
		Equipped = true;
		_armsAnimator.runtimeAnimatorController = _data.ArmsAnimator;

		OnEquip();
	}

	public void Unequip()
	{
		OnUnequip();

		Equipped = false;
		gameObject.SetActive(false);
	}

	protected virtual void OnEquip() { }
	protected virtual void OnUnequip() { }
}