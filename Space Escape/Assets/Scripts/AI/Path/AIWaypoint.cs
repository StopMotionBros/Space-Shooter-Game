using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIWaypoint : MonoBehaviour
{
	public AIWaypoint Next;
	public AIWaypoint Previous;
	[Range(0.1f, 15)] public float Width;

	[Range(0, 1)] public float BranchChance;
	public List<AIWaypoint> Branches = new();

	public Vector3 GetPosition()
	{
		//Vector3 right = _width * transform.right;
		return transform.position;
	}

	public AIWaypoint Branch()
	{
		float rand = Random.value;

		if (rand > BranchChance) return this;
		else return Branches[Random.Range(0, Branches.Count)];
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Vector3 right = Width * transform.right;
		Gizmos.DrawLine(transform.position - right, transform.position + right);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position - right, 0.1f);
		Gizmos.DrawSphere(transform.position + right, 0.1f);

		Gizmos.color = Color.white;
		if (Next)
		{
			Vector3 nextRight = Next.Width * Next.transform.right;
			Gizmos.DrawLine(transform.position - right, Next.transform.position - nextRight);
		}

		Gizmos.color = Color.white;
		if (Previous)
		{
			Vector3 previousRight = Previous.Width * Previous.transform.right;
			Gizmos.DrawLine(transform.position + right, Previous.transform.position + previousRight);
		}

		Gizmos.color = Color.black;
		DrawText(transform.position + 0.5f * Vector3.up, name);

		if (Branches.Count == 0) return;

		foreach (AIWaypoint branch in Branches)
		{
			Gizmos.DrawLine(transform.position, branch.transform.position);
			Gizmos.DrawSphere(branch.transform.position, 0.1f);
		}
	}

	static void DrawText(Vector3 position, string text)
	{
		GUI.color = Gizmos.color;

		Handles.Label(position, text);
	}
}