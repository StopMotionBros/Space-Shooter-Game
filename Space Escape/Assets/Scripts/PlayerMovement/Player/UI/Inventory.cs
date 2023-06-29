using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour
{
	public Action<InventorySlot> OnSlotSelected;

	[SerializeField] PlayerController _player;
	[SerializeField] GameObject _content;

	[Space]

	[SerializeField] InventorySlot _slotPrefab;

	[Space]

	[SerializeField] short _maxWeight;
	short _currentWeight;

	[SerializeField] List<InventorySlot> _slots = new();
	[SerializeField] Transform _container;

	[Space]

	[SerializeField] TMP_Text _capacityText;

	public InventorySlot SelectedSlot => _selectedSlot;
	InventorySlot _selectedSlot;

	bool _open;

	Item[] _items;

	void Start()
	{
		_items = _player.ItemHolder.GetComponentsInChildren<Item>();
		foreach (Item item in _items)
		{
			item.Init(_player);
		}

		SubscribeInputs();
	}

	#region Input Setup
	void OnEnable()
	{
		if (_player.Actions == null) return;

		SubscribeInputs();
	}

	void OnDisable()
	{
		UnsubscribeInputs();
	}

	void SubscribeInputs()
	{
		_player.Actions.UI.ToggleInventory.performed += OnToggleInventory;
	}

	void UnsubscribeInputs()
	{
		_player.Actions.UI.ToggleInventory.performed -= OnToggleInventory;
	}
	#endregion

	void OnToggleInventory(InputAction.CallbackContext context)
	{
		_open = !_open;
		_content.SetActive(_open);
		GameManager.ToggleCursor(_open);
	}

	public bool AddItem(ItemData data, short amount, out short remainder)
	{
		remainder = amount;
		if (data.Weight + _currentWeight > _maxWeight) return false;
		if (data == null) return false;

		bool addedItem = false;

		if (_slots.Count > 0 && data.Stackable)
		{
			foreach (InventorySlot slot in _slots)
			{
				for (short i = 0; i < amount; i++)
				{
					if (slot.AddItem(data, 1))
					{
						_currentWeight += data.Weight;
						addedItem = true;
						remainder--;
					}
					if (_currentWeight + data.Weight > _maxWeight) break;
				}
			}
		}

		if (!addedItem)
		{
			InventorySlot newSlot = Instantiate(_slotPrefab, _container);
			newSlot.Init(data, 1, this, GetItemFromData(data));
			_currentWeight += data.Weight;
			remainder--;

			if (amount > 1)
			{
				for (short i = (short)(amount - 2); i >= 0; i--)
				{
					if (newSlot.AddItem(data, 1))
					{
						_currentWeight += data.Weight;
					}
					else AddItem(data, (short)(i - 1), out remainder);

					if(_currentWeight + data.Weight > _maxWeight) break;

					remainder--;
				}
			}

			_slots.Add(newSlot);
		}

		UpdateUI();

		return true;
	}

	public bool Remove(ItemData item, short amount, out short remainder)
	{
		remainder = amount;
		foreach (InventorySlot slot in _slots)
		{
			if(slot.Slot.Data.Name == item.Name)
			{
				slot.Slot.RemoveAmount(amount, out remainder);
				if (remainder > 0) Remove(item, remainder, out remainder);
				slot.UpdateSlot();
				if (slot.Slot.Empty) _slots.Remove(slot);
				return true;
			}
		}

		return false;
	}

	public void Select(InventorySlot slot)
	{
		_selectedSlot = slot;
		EventSystem.current.SetSelectedGameObject(slot.gameObject);

		OnSlotSelected?.Invoke(slot);
	}

	public bool Drop(InventorySlot slot, short amount)
	{
		if (slot.Slot.Data.Important) return false;
		if (slot == null) return false;
		if (slot.Slot.Empty) { slot.Clear(); return false; }

		_currentWeight -= (short)(slot.Slot.Data.Weight * amount);
		slot.Drop(amount);
		if (slot.Slot.Empty) _slots.Remove(slot);

		UpdateUI();

		return true;
	}

	public bool DropSelected(short amount)
	{
		return Drop(_selectedSlot, amount);
	}

	Item GetItemFromData(ItemData data)
	{
		Item itemFromData = null;
		foreach (Item item in _items)
		{
			if(data.Name == item.Data.Name)
			{
				itemFromData = item;
				break;
			}
		}

		return itemFromData;
	}

	void UpdateUI()
	{
		_capacityText.SetText(_currentWeight + "/" + _maxWeight + "{lbs}");
	}
}
