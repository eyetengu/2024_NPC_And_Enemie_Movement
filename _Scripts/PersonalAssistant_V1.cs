using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class PersonalAssistant_V1 : MonoBehaviour
{
    private enum AIState
    {
        Idle,
        FollowPlayer,
        ProtectPlayer,
        HealPlayer,
        Death
    }

    private NavMeshAgent _agent;

    [SerializeField] private AIState _currentState;
    [SerializeField] private Player _playerScript;

    private bool _healing;
    private int _healingMultiplier = 5;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _enemyTransform;

    //Ranges
    [SerializeField] private float _followRange = 3.5f;
    [SerializeField] private float _threatRange;
    [SerializeField] private float _intimateRange;
    [SerializeField] private float _engagementRange;
    private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 4;
    private float _step;
    private float _rotationStep;
    private bool _playerNeedsAttention;
    private bool _inRange;
    private bool _enemyInRange;
    private bool _canAttack;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _canAttack = true;
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
        ValetStateMachine();
        
    }

    //PLAYER BASED LOGIC
    void PlayerDistanceChecker()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        var enemyDistanceToPlayer = Vector3.Distance(_playerTransform.position, _enemyTransform.position);
        
        if (distanceToPlayer > _followRange)                        //Follow Player
        {
            _currentState = AIState.FollowPlayer;
            _inRange= false;
                
        }
        if(distanceToPlayer < _followRange)                         //Chill and Watch Player
        {
            _currentState = AIState.Idle;
            _inRange = true;
        }
        
        PlayerStatusCheck();                                        //Status Checker

        if (_inRange)                                       //IF ASSISTANT IS WITHIN RANGE OF PLAYER 
        {
            if (enemyDistanceToPlayer < _followRange)               //Protect Player
            {
                _currentState = AIState.ProtectPlayer;
                
            }

             else if (_playerNeedsAttention)                        //Heal Player
            {
                //MoveToHealPlayer();
                //_currentState = AIState.Idle;
                //PlayerStatusCheck();
                
            }
            else
                _currentState = AIState.Idle;                       //Chill and Watch Player
        }       
    }

    private void PlayerStatusCheck()
    {
        var playerHealth = _playerScript.Health;
        Debug.Log("Player Health: " + playerHealth);
        //check the player health status
        //if the player is not at maximum health then
        //the personal assistant can move closer
        //and heal player

        Debug.Log("Player" + _playerScript.Health);

    }

    void ValetStateMachine()
    {
        switch (_currentState)
        {
            case AIState.Idle:
                Debug.Log("Idling");

                _agent.isStopped = true;
                RotateTowardPlayer();
                break;

            case AIState.FollowPlayer:
                Debug.Log("Following Player");

                FollowPlayer();

                break;

            case AIState.ProtectPlayer:
                Debug.Log("Protecting Player");

                EngageEnemy();
                if(_enemyInRange)
                {
                    AttackEnemy();
                }
                //ProtectPlayer();

                break;

            case AIState.HealPlayer:
                Debug.Log("Healing Player");

                if (_healing == false)
                {
                    _healing = true;
                    HealPlayer();
                    StartCoroutine(HealingRoutine());
                }

                break;

            case AIState.Death:
                Debug.Log("Dead");

                break;

            default:
                Debug.Log("Invalid Enemy State");

                break;
        }
    }

    //Enemy_Specific Logic
   
    private void EngageEnemy()
    {
        RotateTowardEnemy();
        MoveTowardEnemy();
    }

    private void RotateTowardEnemy()
    {
        Vector3 targetDirection = _enemyTransform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotationStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.DrawRay(transform.position, newDirection, Color.red);
    }

    private void MoveTowardEnemy()
    {
        var distanceToEngage = Vector3.Distance(transform.position, _enemyTransform.position);

        if (distanceToEngage > 2)
            transform.position = Vector3.MoveTowards(transform.position, _enemyTransform.position, _step);
        else
            AttackEnemy();
    }

    //Player-Specific Logic
    void FollowPlayer()
    {
        MoveTowardPlayer();
        RotateTowardPlayer();
    }

    void AttackEnemy()
    {
        if(_canAttack)
        {
            _canAttack= false;
            //Knock off enemy health
            StartCoroutine(AttackRoutine());
        }
    }


    void HealPlayer()
    {
        _playerScript.AddHealth(_healingMultiplier);

        if(_playerScript.Health < _playerScript.MaxPlayerHealth)
        {
            Debug.Log("Healing");
        }
        if(_playerScript.Health >= _playerScript.maxPlayerHealth)
        {
            _playerScript.Health = _playerScript.MaxPlayerHealth;
        }
        
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

    


   


   

   
    //COROUTINES
    IEnumerator AttackRoutine()
    {
        _agent.isStopped = true;
        yield return new WaitForSeconds(3);
        _agent.isStopped = false;
        _currentState = AIState.Idle;
        _canAttack = false;
        _agent.isStopped = false;
    }
    IEnumerator HealingRoutine()
    {
        _agent.isStopped = true;
        yield return new WaitForSeconds(3);
        _agent.isStopped = false;
        _currentState = AIState.Idle;
        _healing = false;
        _agent.isStopped = false;
    }
}
