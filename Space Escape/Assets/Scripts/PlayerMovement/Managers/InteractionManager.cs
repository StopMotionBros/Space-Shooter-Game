using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] PlayerController _player;
	Transform _cameraHolder;

	[Space]

	[SerializeField] float _maxInteractDistance;
	[SerializeField] LayerMask _interactible;

	[Space]

	[SerializeField] Image _crossHair;

	NativeArray<RaycastCommand> _raycastCommands;
	NativeArray<RaycastHit> _raycastHits;
	JobHandle _job;

	void Start()
	{
		_cameraHolder = _player.CameraHolder;

		_raycastCommands = new(1, Allocator.Persistent);
		_raycastHits = new(1, Allocator.Persistent);

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
		_player.Actions.ItemInteraction.Interact.performed += Interact;
	}

	void UnsubscribeInputs()
	{
		_player.Actions.ItemInteraction.Interact.performed -= Interact;
	}
	#endregion

	void Update()
	{
		_job.Complete();

		RaycastHit hit = _raycastHits[0];
		if(hit.collider != null)
		{
			if (hit.collider.CompareTag("Enemy"))
				_crossHair.color = Color.red;
			else if (hit.distance <= _maxInteractDistance && hit.collider.CompareTag("Interactable"))
				_crossHair.color = Color.green;
			else
				_crossHair.color = Color.white;
		}
		else _crossHair.color = Color.white;
		
		_raycastCommands[0] = new RaycastCommand(_cameraHolder.position, _cameraHolder.forward, new QueryParameters(_interactible), 100);
		_job = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
	}

	void Interact(InputAction.CallbackContext context)
	{
		if(Physics.Raycast(_cameraHolder.position, _cameraHolder.forward, out RaycastHit hit, _maxInteractDistance, _interactible))
		{
			if (!hit.collider.CompareTag("Interactable")) return;
			if (hit.collider.TryGetComponent(out IInteractable interactable)) interactable.Interact(_player);
		}
	}

	void OnDestroy()
	{
		_job.Complete();
		_raycastCommands.Dispose();
		_raycastHits.Dispose();
	}
}
