using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public ItemData Data => _data;
	[SerializeField] protected ItemData _data;
	[SerializeField] protected PlayerController _player;

	public void Init(PlayerController player)
	{
		_player = player;
		Init();
	}

	public abstract void Use();
	protected virtual void Init() { }
}