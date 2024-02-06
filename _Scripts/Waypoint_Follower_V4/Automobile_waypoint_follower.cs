using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automobile_waypoint_follower : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    private int _waypointID;

    [SerializeField] private Transform[] _driverWheels;
    [SerializeField] private Transform[] _passengerWheels;
    [SerializeField] private float _wheelSpeed = 150;
    private float _step;
    [SerializeField] private float _speed = 5;


    private void Start()
    {
        
    }

    void LateUpdate()
    {
        _step = _speed * Time.deltaTime;
        //SelectNextWaypoint();
        FollowWaypointsAndDump();
    }

    void SelectNextWaypoint()
    {
        _waypointID++;
        
        if (_waypointID > _waypoints.Length - 1)
        {
            _waypointID = 0;
        }

        //targetWaypoint = _waypoints[_waypointID];                   //create a variable and store current waypoint's transform
    }

    void RotateWheels()
    {
        if (_driverWheels != null)
        {
            foreach (var wheel in _driverWheels)
            {
                wheel.Rotate(_wheelSpeed * Time.deltaTime, 0, 0);
            }
        }
        if(_passengerWheels != null)
        {
            foreach (var wheel in _passengerWheels)
            {
                wheel.Rotate(_wheelSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    void FollowWaypointsAndDump()
    {
        //Debug.Log("Following Waypoints");
        if(_waypoints != null)
        {
            var targetWaypoint = _waypoints[_waypointID];                   //create a Move towards waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.transform.position, _step);

            //Rotate to look at waypoint
            Vector3 targetDirection = _waypoints[_waypointID].position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            var distance = Vector3.Distance(transform.position, targetWaypoint.transform.position);

            if (distance < .1f)
                SelectNextWaypoint();

            if (_waypointID > _waypoints.Length - 1)
            {
                _waypointID = 0;
            }

            RotateWheels();
        }
    }

}
