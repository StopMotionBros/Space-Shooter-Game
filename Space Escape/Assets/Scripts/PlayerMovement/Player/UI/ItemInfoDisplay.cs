using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemInfoDisplay : MonoBehaviour
{
	[SerializeField] Inventory _inventory;

	[Space]

	[SerializeField] TMP_Text _name;
	[SerializeField] TMP_Text _description;

	[Space]

	[SerializeField] TMP_Text _weightText; 
	[SerializeField] TMP_Text _amountText;
	[SerializeField] Slider _amountSlider;

	[SerializeField] CanvasGroup _buttons;
	[SerializeField] Button _equipButton;
	[SerializeField] CanvasGroup _dropButton;

	[Space]

	[SerializeField] GameObject _dropModal;

	InventorySlot _selectedSlot;
	EquippableItem _equippedSlot;

	void Awake()
	{
		ResetUI();

		_inventory.OnSlotSelected += (slot) =>
		{
			ToggleDropModal(false);

			_amountSlider.minValue = 1;
			_amountSlider.maxValue = slot.Slot.Amount;
			_amountText.SetText(_amountSlider.value + "/" + slot.Slot.Amount);
			Select(slot.Slot.Data);

			if (!_buttons.interactable) _buttons.interactable = true;

			_equipButton.interactable = slot.Slot.Data.CanUse;
			_dropButton.interactable = !slot.Slot.Data.Important;

			_selectedSlot = slot;
		};
		_amountSlider.onValueChanged.AddListener(v => 
		{
			if (_selectedSlot)
			{
				_weightText.SetText($"{v * _selectedSlot.Slot.Data.Weight}{{lbs}}");
				_amountText.SetText(v + "/" + _amountSlider.maxValue);
			}
			else ResetUI();
		});
	}

	public void Select(ItemData item)
	{
		StopAllCoroutines();
		StartCoroutine(AnimateText(_name, item.Name, "// ", " //"));
		StartCoroutine(AnimateText(_description, item.Description, "<", ">"));
	}

	IEnumerator AnimateText(TMP_Text textObj, string text, string start = "", string end = "")
	{
		textObj.SetText(start + end);
		int startLength = start.Length;
		char[] chars = text.ToCharArray();

		for (int i = 0; i < text.Length; i++)
		{
			textObj.SetText(textObj.text.Insert(startLength++, chars[i].ToString()));
			yield return new WaitForEndOfFrame();
		}
	}

	public void ToggleDropModal()
	{
		ToggleDropModal(!_dropModal.activeSelf);
	}

	public void ToggleDropModal(bool enabled)
	{
		_dropModal.SetActive(enabled);
	}

	void ResetUI()
	{
		ToggleDropModal(false);
		_buttons.interactable = false;

		_name.SetText("// //");
		_description.SetText("<>");
	}

	public void Use()
	{
		if (!_selectedSlot) return;
		if (!_selectedSlot.Slot.Data.CanUse) return;
		bool canUse;

		if (_selectedSlot.Item is EquippableItem)
		{
			if (_equippedSlot) _equippedSlot.Unequip();
			_equippedSlot = _selectedSlot.Item as EquippableItem;
			canUse = true;
		}
		else canUse = _inventory.Remove(_selectedSlot.Slot.Data, 1, out _);
		if (canUse) _selectedSlot.Item.Use();
	}

	public void Drop()
	{
		if(_inventory.DropSelected((short)_amountSlider.value)) ResetUI();
		ToggleDropModal(false);
	}
}