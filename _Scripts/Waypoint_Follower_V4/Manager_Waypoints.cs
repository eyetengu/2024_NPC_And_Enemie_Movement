using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Waypoints : MonoBehaviour
{
    [SerializeField] Transform[] _waypointsWalking;
    [SerializeField] Transform[] _waypointsDriving;
    [SerializeField] Transform[] _waypointsFlying;


    public Transform ChooseRandom_StartWaypoint_Walking()
    {
        var randomWaypointID = Random.Range(0, _waypointsWalking.Length - 1);
        var waypointTransform = _waypointsWalking[randomWaypointID];
        //Debug.Log("Waypoint: " + waypointTransform + " chosen");
        return waypointTransform;
    }

    public Transform ChooseRandom_StartWaypoint_Driving()
    {
        var randomWaypoint = Random.Range(0, _waypointsDriving.Length - 1);
        var waypointTransform = _waypointsDriving[randomWaypoint];
        return waypointTransform;
    }

    public Transform ChooseRandom_StartWaypoint_Flying()
    {
        var randomWaypoint = Random.Range(0, _waypointsFlying.Length - 1);
        var waypointTransform = _waypointsFlying[randomWaypoint];
        return waypointTransform;
    }

    public Transform[] ReturnDrivingWaypoints()
    {
        return _waypointsDriving;
    }
}
