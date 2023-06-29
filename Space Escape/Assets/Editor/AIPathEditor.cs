using UnityEditor;
using UnityEngine;

public class AIPathEditor : EditorWindow
{
	AIPath _path;

    [MenuItem("Tools/Path Editor")]
    public static void Open()
    {
        GetWindow<AIPathEditor>("Path Editor");
    }

	void OnGUI()
	{
		SerializedObject obj = new SerializedObject(this);

		_path = EditorGUILayout.ObjectField("Path", _path, typeof(AIPath), true) as AIPath;

		if (_path == null)
		{
			EditorGUILayout.HelpBox("Select a transform object.", MessageType.Warning);
		}
		else
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical();
			DrawButtons();
			EditorGUILayout.EndVertical();
		}

		obj.ApplyModifiedProperties();
	}

	void DrawButtons()
	{
		if (GUILayout.Button("Add AIWaypoint"))
		{
			CreateWaypoint();
		}
		if (Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent(out AIWaypoint waypoint))
		{
			EditorGUILayout.Space();

			if (GUILayout.Button("Create Branch"))
			{
				CreateBranch();
			}
			if (GUILayout.Button("Create AIWaypoint After"))
			{
				CreateWaypointAfter();
			}
			if (GUILayout.Button("Create AIWaypoint Before"))
			{
				CreateWaypointBefore();
			}
			if (GUILayout.Button("Remove AIWaypoint"))
			{
				RemoveWaypoint();
			}

			EditorGUILayout.Space();

			waypoint.Width = EditorGUILayout.Slider("Width", waypoint.Width, 0.1f, 15);
			waypoint.BranchChance = EditorGUILayout.Slider("Branch Chance", waypoint.BranchChance, 0, 1);

			EditorGUILayout.Space();

			waypoint.Next = EditorGUILayout.ObjectField("Next", waypoint.Next, typeof(AIWaypoint), true) as AIWaypoint;
			waypoint.Previous = EditorGUILayout.ObjectField("Previous", waypoint.Previous, typeof(AIWaypoint), true) as AIWaypoint;
		}
	}

	void CreateWaypoint()
	{
		GameObject waypointObj = new GameObject(_path.transform.childCount.ToString(), typeof(AIWaypoint));
		waypointObj.transform.SetParent(_path.transform);

		AIWaypoint waypoint = waypointObj.GetComponent<AIWaypoint>();
		if (_path.transform.childCount > 1)
		{
			waypoint.Previous = _path.transform.GetChild(_path.transform.childCount - 2).GetComponent<AIWaypoint>();
			waypoint.Previous.Next = waypoint;

			waypoint.transform.position = waypoint.Previous.transform.position;
			waypoint.transform.forward = waypoint.Previous.transform.forward;

			waypoint.Width = waypoint.Previous.Width;
		}

		Selection.activeObject = waypointObj;

		_path.Waypoints.Add(waypoint);
	}

	void CreateWaypointAfter()
	{
		GameObject waypointObj = new GameObject(_path.transform.childCount.ToString(), typeof(AIWaypoint));
		waypointObj.transform.SetParent(_path.transform, false);

		AIWaypoint newWaypoint = waypointObj.GetComponent<AIWaypoint>();

		AIWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<AIWaypoint>();

		newWaypoint.transform.position = selectedWaypoint.transform.position;
		newWaypoint.transform.forward = selectedWaypoint.transform.forward;

		if (selectedWaypoint.Previous != null)
		{
			newWaypoint.Previous = selectedWaypoint.Previous;
			selectedWaypoint.Next = newWaypoint;
		}

		newWaypoint.Next = selectedWaypoint;

		selectedWaypoint.Previous = newWaypoint;

		newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

		newWaypoint.Width = selectedWaypoint.Width;

		Selection.activeObject = waypointObj;

		_path.Waypoints.Add(newWaypoint);
	}

	void CreateWaypointBefore()
	{
		GameObject waypointObj = new GameObject(_path.transform.childCount.ToString(), typeof(AIWaypoint));
		waypointObj.transform.SetParent(_path.transform, false);

		AIWaypoint newWaypoint = waypointObj.GetComponent<AIWaypoint>();

		AIWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<AIWaypoint>();

		newWaypoint.transform.position = selectedWaypoint.transform.position;
		newWaypoint.transform.forward = selectedWaypoint.transform.forward;

		newWaypoint.Previous = selectedWaypoint;

		if (selectedWaypoint.Next != null)
		{
			selectedWaypoint.Next.Previous = newWaypoint;
			newWaypoint.Next = selectedWaypoint.Next;
		}

		selectedWaypoint.Next = newWaypoint;

		newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

		newWaypoint.Width = selectedWaypoint.Width;

		Selection.activeObject = waypointObj;

		_path.Waypoints.Add(newWaypoint);
	}

	void RemoveWaypoint()
	{
		AIWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<AIWaypoint>();
		_path.Waypoints.Add(selectedWaypoint);

		if (selectedWaypoint.Next != null)
		{
			selectedWaypoint.Next.Previous = selectedWaypoint.Previous;
		}
		if (selectedWaypoint.Previous != null)
		{
			selectedWaypoint.Previous.Next = selectedWaypoint.Next;
			Selection.activeGameObject = selectedWaypoint.Previous.gameObject;
		}

		DestroyImmediate(selectedWaypoint.gameObject);
	}

	void CreateBranch()
	{
		GameObject waypointObj = new GameObject(_path.transform.childCount.ToString(), typeof(AIWaypoint));
		waypointObj.transform.SetParent(_path.transform, false);

		AIWaypoint waypoint = waypointObj.GetComponent<AIWaypoint>();

		AIWaypoint branchedFrom = Selection.activeGameObject.GetComponent<AIWaypoint>();
		branchedFrom.Branches.Add(waypoint);

		waypoint.transform.position = branchedFrom.transform.position;
		waypoint.transform.forward = branchedFrom.transform.forward;

		Selection.activeGameObject = waypoint.gameObject;

		_path.Waypoints.Add(waypoint);
	}
}
