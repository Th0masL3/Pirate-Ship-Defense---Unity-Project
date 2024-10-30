using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform[] waypoints;

    public Vector2 getWaypoint(int index)
    {
        return waypoints[index].position;
    }

    public float getCumulitveDistance(int index, Vector2 position)
    {
        float distace = Vector2.Distance(position, waypoints[index].position);
        for (int i = index; i < waypoints.Length - 1; i++)
        {
            distace += Vector2.Distance(waypoints[i].position, waypoints[i + 1].position);
        }

        return distace;
    }
    /*
    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            //zmos.color = Color.red;
          //Gizmos.DrawSphere(waypoints[i].position, 0.2f);
            if (i > 0)
                Gizmos.DrawLine(waypoints[i-1].position, waypoints[i].position);
        }
    }*/
}
