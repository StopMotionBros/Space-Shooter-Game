using System.Collections.Generic;
using UnityEngine;

public class AIPath : MonoBehaviour
{
    public List<AIWaypoint> Waypoints;

    public AIWaypoint this[int i] => Waypoints[i];

    public AIWaypoint GetClosest(Vector3 position)
    {
        AIWaypoint waypoint = null;
        float sqrDistance = float.MaxValue;

        foreach (AIWaypoint w in Waypoints)
        {
            float distance = (w.transform.position - position).sqrMagnitude;
			if (distance < sqrDistance)
            {
                waypoint = w;
                sqrDistance = distance;
            }
        }

        return waypoint;
    }
}
