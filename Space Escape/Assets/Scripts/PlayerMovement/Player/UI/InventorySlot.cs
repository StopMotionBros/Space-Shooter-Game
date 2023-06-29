using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

using Random = UnityEngine.Random;

public class InventorySlot : Selectable
{
	Inventory _inventory;
	PlayerController _player;

	public ref SlotData Slot => ref _slot;
	[SerializeField] SlotData _slot;
	
	[Space]

	[SerializeField] TMP_Text _name;
	[SerializeField] TMP_Text _amount;
	[SerializeField] TMP_Text _weight;

	public Item Item => _item;
	Item _item;

	protected override void Start()
	{
		base.Start();

		UpdateSlot();

		_player = transform.root.GetComponent<PlayerController>();
	}

	public void Init(ItemData data, short amount, Inventory inventory, Item item)
	{
		_inventory = inventory;
		_slot = new SlotData(data, amount);
		_item = item;
		UpdateSlot();
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);

		_inventory.Select(this);
	}

	public bool AddItem(ItemData item, short amount)
	{
		if (item == null) return false;

		if (_slot.Empty) { Clear(); return false; }
		if (!_slot.Stackable) return false;
		if (_slot.Data.Name != item.Name) return false;

		bool addedItem = _slot.AddAmount(amount);

		UpdateSlot();
		return addedItem;
	}

	public void UpdateSlot()
	{
		if (_slot.Empty)
		{
			Clear();
		}
		else
		{
			_name.SetText(_slot.Data.Name);
			_amount.SetText(_slot.Amount.ToString());
			_weight.SetText(_slot.Data.Weight * _slot.Amount + "{lbs}");
		}
	}

	public void Clear()
	{
		if(gameObject != null) Destroy(gameObject);
	}

	public void Drop(short amount)
	{
		ItemPickup pickup = Instantiate(_slot.Data.DropPrefab, _player.transform.position + Vector3.up, Random.rotation);

		_slot.RemoveAmount(amount, out short remainder);
		pickup.Amount = (short)(amount - remainder);

		if (_slot.Empty) Clear();

		UpdateSlot();
	}
}
[Serializable]
public struct SlotData
{
    public ItemData Data => _data;
	[SerializeField] ItemData _data;

	public short Amount => _amount;
    [SerializeField] short _amount;

	public bool Stackable => _data.Stackable;

	public short Weight => (short)(_data.Weight * _amount);

	public bool Empty => _amount <= 0 || _data == null;

	public SlotData(ItemData data, short amount)
	{
		_data = data;
		_amount = amount;
	}

	public void Clear()
	{
		_data = null;
		_amount = 0;
	}

	public bool SetAmount(short amount)
	{
		if (Empty) { Clear(); return false; }
		if (Stackable)
		{
			_amount = amount;
			return true;
		}
		return false;
	}

	public bool AddAmount(short amount)
	{
		if(_data.Stackable)
		{
			_amount += amount;
			return true;
		}
		return false;
	}

	public void RemoveAmount(short amount, out short remainder)
	{
		if (amount > _amount)
		{
			remainder = (short)(_amount - amount);
			Clear();
		}
		else
		{
			_amount -= amount;
			remainder = 0;
			if (Empty) Clear();
		}
	}
}
