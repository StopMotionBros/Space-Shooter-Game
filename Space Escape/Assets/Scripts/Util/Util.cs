using UnityEngine;

public static class Util
{
	public static bool ContainsLayer(LayerMask mask, int layer)
	{
		return (mask & (1 << layer)) != 0;
	}
}