using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public static Action<bool> OnCursorToggled;

	void Awake()
	{
		Instance = this;
	}

	public static void ToggleCursor(bool enabled)
	{
		Cursor.visible = enabled;
		Cursor.lockState = enabled ? CursorLockMode.None : CursorLockMode.Locked;

		OnCursorToggled?.Invoke(enabled);
	}
}