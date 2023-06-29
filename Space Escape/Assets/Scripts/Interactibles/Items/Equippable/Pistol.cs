using UnityEngine.InputSystem;
using UnityEngine;

public class Pistol : EquippableItem
{
	[SerializeField] float _range;
	Transform _camHolder;

	[SerializeField] LayerMask _enemy;

	protected override void OnEquip()
	{
		_camHolder = _player.CameraHolder;

		_player.Actions.ItemInteraction.Reload.performed += Reload;
		_player.Actions.ItemInteraction.Use.performed += Use;
	}

	protected override void OnUnequip()
	{
		_player.Actions.ItemInteraction.Reload.performed -= Reload;
		_player.Actions.ItemInteraction.Use.performed -= Use;
	}

	void Use(InputAction.CallbackContext context)
	{
		if(Physics.Raycast(_camHolder.position, _camHolder.forward, out RaycastHit hit, _range, _enemy))
		{
			print(hit.collider.name);
		}
	}

	void Reload(InputAction.CallbackContext context)
	{
		_armsAnimator.Play("Reload");
		_animator.Play("Reload");
	}
}