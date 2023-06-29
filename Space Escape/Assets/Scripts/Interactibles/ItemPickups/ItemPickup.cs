using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
	[SerializeField] ItemData _data;
	public short Amount;

	public virtual void Interact(PlayerController interacter)
	{
		if (interacter.Inventory.AddItem(_data, Amount, out Amount))
		{
			if (Amount <= 0) Destroy(gameObject);
		}
	}
}