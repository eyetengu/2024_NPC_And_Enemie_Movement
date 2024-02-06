using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_V1 : MonoBehaviour
{
    //This version of enemy AI will have the following states: idle, noticePlayer, coverPlayer, attackPlayer and death
    //alternative versions can include chase, engage, flee


    private enum AIState
    {
        Idle,
        NoticePlayer,
        CoverPlayer,
        AttackPlayer,
        Death
    }

    private NavMeshAgent _agent;
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private bool _isRandom;
    private bool _readyToSelectPoint;
    private int _randomTarget;

    private int _currentPoint;
    private Transform _currentTarget;

    private bool _inReverse;
    [SerializeField] private AIState _currentState;
    private bool _attacking;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _awarenessRange;
    [SerializeField] private float _cautionRange;
    [SerializeField] private float _engagementRange;
    private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 4;
    private float _step;
    private float _rotationStep;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        _step = _speed * Time.deltaTime;
        _rotationStep = _rotationSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //_currentState = AIState.Jumping;
            _agent.isStopped = true;
        }

        PlayerDistanceChecker();

        switch (_currentState)
        {
            case AIState.Idle:
                Debug.Log("Idling");
                break;

            case AIState.NoticePlayer:
                Debug.Log("Noticing Player");
                RotateTowardPlayer();
                _agent.isStopped = true;
                break;

            case AIState.CoverPlayer:
                Debug.Log("Covering Player");
                RotateTowardPlayer();
                _agent.isStopped = true;
                break;

            case AIState.AttackPlayer:
                Debug.Log("Attacking Player");
                RotateTowardPlayer();
                if (_attacking == false)
                    _attacking = true;
                _agent.isStopped = true;
                StartCoroutine(AttackRoutine());
                break;

            case AIState.Death:
                Debug.Log("Dead");
                break;

            default:
                Debug.Log("Invalid Enemy State");
                break;
        }
    }

    //PLAYER BASED LOGIC
    void PlayerDistanceChecker()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer > _awarenessRange)
        {
            _currentState = AIState.Idle;
        }

        if (distanceToPlayer < _awarenessRange)
        {
            Debug.Log("Boo");
            _currentState = AIState.NoticePlayer;
        }

        if (distanceToPlayer < _cautionRange)
        {
            Debug.Log("Ya!");
            _currentState = AIState.CoverPlayer;
        }

        if (distanceToPlayer < _engagementRange)
        {
            Debug.Log("Hi-Ya");
            _currentState = AIState.AttackPlayer;
        }
    }

    void ChasePlayer()
    {
        MoveTowardPlayer();
        RotateTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _step);
    }

    void RotateTowardPlayer()
    {
        Vector3 targetDirection = _playerTransform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotationStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.DrawRay(transform.position, newDirection, Color.yellow);
    }

    //WAYPOINT LOGIC
    void ChooseWaypointFollowMethod()
    {
        if (_isRandom)
        {
            ChooseRandomLogic();
        }
        else
            CalculateAIMovement();
    }


    //ENEMY WAYPOINT FOLLOWING METHODS
    void ChooseRandomLogic()
    {
        if (_readyToSelectPoint)
        {
            _readyToSelectPoint = false;
            ChooseRandomPoint();
        }

        if (_agent.remainingDistance < 0.5f)
        {
            _readyToSelectPoint = true;
        }

        _agent.destination = _waypoints[_randomTarget].position;
    }
    void ChooseRandomPoint()
    {
        _randomTarget = Random.Range(0, _waypoints.Count);

        if (_agent != null)
        {
            _agent.destination = _waypoints[_randomTarget].position;
        }
    }

    void CalculateAIMovement()
    {
        if (_agent.remainingDistance < 0.5f)
        {
            _currentState = AIState.AttackPlayer;
            if (_inReverse)
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
        if (_currentPoint == _waypoints.Count - 1)
        {
            _inReverse = true;
            _currentPoint--;
        }
        else
        {
            _currentPoint++;
        }
    }


    //COROUTINES
    IEnumerator AttackRoutine()
    {
        _agent.isStopped = true;
        yield return new WaitForSeconds(3);
        _agent.isStopped = false;
        _currentState = AIState.Idle;
        _attacking = false;
        _agent.isStopped = false;
    }
}
