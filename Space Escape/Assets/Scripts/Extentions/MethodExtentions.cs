using Unity.Mathematics;
using UnityEngine;

public static class MethodExtentions
{
	public static float3 ToFloat3(this Vector3 vector)
	{
		return vector;
	}

	public static Vector3 ToVector3(this float3 vector)
	{
		return vector;
	}

	public static Vector2 ToVector2(this Vector2Int vector)
	{
		return vector;
	}

	public static Vector2Int ToVector2Int(this Vector2 vector)
	{
		return new Vector2Int((int)vector.x, (int)vector.y);
	}

	public static Vector2Int FloorToVector2Int(this Vector2 vector)
	{
		return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
	}
}