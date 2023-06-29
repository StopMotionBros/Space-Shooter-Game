using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
	public bool CanUse => _canUse;
	[SerializeField] bool _canUse;

	public bool Important => _important;
	[SerializeField] bool _important;

	public ItemPickup DropPrefab => _dropPrefab;
	[Space][SerializeField] ItemPickup _dropPrefab;

	public string Name => _name;
	[Space][SerializeField] string _name;

	public string Description => _description;
	[SerializeField, TextArea(3, 15)] string _description;

	public bool Stackable => _stackable;
	[Space][SerializeField] bool _stackable;

	public short Weight => _weight;
	[Space][SerializeField] short _weight;

	public RuntimeAnimatorController ArmsAnimator => _armsAnimator;
	[Space][SerializeField] RuntimeAnimatorController _armsAnimator;
}
