using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Waypoints : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private bool _isRandom;
    private bool _readyToSelectPoint;
    private int _randomTarget;
    private int _currentPoint;
    private bool _inReverse;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if(_isRandom)
        {
            CalculateRandomFollower();
        }
        else
            CalculateAIMovement();
    }

    void ChooseRandomPoint()
    {
        _randomTarget = Random.Range(0, _waypoints.Count);

        if(_agent != null)
        {
            _agent.destination = _waypoints[_randomTarget].position;
        }
    }

    void CalculateRandomFollower()
    {
        if(_readyToSelectPoint)
        {
            _readyToSelectPoint = false;
            ChooseRandomPoint();
        }

        if(_agent.remainingDistance < 0.5f)
        {
            _readyToSelectPoint = true;
        }

        _agent.destination = _waypoints[_randomTarget].position;
    }

    void CalculateAIMovement()
    {
        if(_agent.remainingDistance < 0.5f)
        {
            if(_inReverse)
            {
                Reverse();
            }
            else
            {
                Forward();
            }
            _agent.SetDestination(_waypoints[_currentPoint].position);
        }
    }

    void Reverse()
    {
        if (_currentPoint == 0)
        {
            _inReverse = false;
            _currentPoint++;
        }
        else
        {
            _currentPoint--;
        }
    }

    void Forward()
    {
        if(_currentPoint == _waypoints.Count -1)
        {
            _inReverse = true;
            _currentPoint--;
        }
        else
        {
            _currentPoint++; 
        }
    }
}
