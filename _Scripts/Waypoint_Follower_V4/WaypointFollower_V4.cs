using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower_V4 : MonoBehaviour
{
    private Manager_Waypoints _waypointManager;

    [SerializeField] private Transform _waypointStart;
    private int _waypointID;
    private Transform _currentWaypoint;

    [SerializeField] private float _speed = 4;
    [SerializeField] private float _rotationSpeed = 10;
    private float _step;
    private float _rotationStep;
    [SerializeField] private Transform _characterModel;


    void Start()
    {      
        _waypointManager = FindObjectOfType<Manager_Waypoints>();

        if(_waypointManager == null)
        {
            Debug.Log("unable to locate waypoint manager");
        }
        else
        {
            if (_waypointStart == null)
            {
                _currentWaypoint = _waypointManager.ChooseRandom_StartWaypoint_Walking();
                //transform.position = _currentWaypoint.position;
            }
            else
            {
                _currentWaypoint = _waypointStart.transform;
            }
        }        
    }

    void Update()
    {
        _step = _speed * Time.deltaTime;
        _rotationStep = _rotationSpeed * Time.deltaTime;

        //Debug.Log(_step + "," + _rotationStep);
        MoveTowardSelectedWaypoint();
    }

    void MoveTowardSelectedWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.transform.position, _step);

        Vector3 targetDirection = _currentWaypoint.position - transform.position;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotationStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Waypoint intercepted");
        if (other.tag == "Waypoint")
        {

            var areaMap = other.GetComponent<AreaMap_New>();

            if (areaMap != null)
            {
                Debug.Log("I have my marching orders");
                var options = areaMap.HereIsTheInformationYouHaveRequested();
                //foreach(var area in options)
                //Debug.Log("options " + area.position);
                var chosenPath = options[Random.Range(0, options.Length)];
                //Debug.Log("ChosenPath: " + chosenPath);
                _currentWaypoint = chosenPath;
            }
            else
                Debug.Log("Area Map Is NULL");
        }
    }
}
