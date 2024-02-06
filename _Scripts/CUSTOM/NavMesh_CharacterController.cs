using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class NavMesh_CharacterController : MonoBehaviour
{
    /// <summary>
    /// Using a NavMeshAgent with a CharacterController
    /// by ChazBass
    ///Since the Unity docs, Unity Answers, and the wild are a little vague on this, 
    ///I would like to share how to get a NavMeshAgent and CharacterController to play nice together on an NPC(tested on 5.6). 
    ///This solution allows the agent to still avoid all defined obstacles and other NPC's 
    ///while also respecting other character controllers (such as the player) and any game geometry with colliders on it.
    ///The key to making sure they play nice is this:
    ///this._agent.velocity = this._charControl.velocity;
    ///Thanks to AngryAnt for that little gem in post I found.
    ///If you do not update the nav mesh agent with the velocity of the character controller post move, unpredictable behavior results.
    /// </summary>
    
    public Transform _playerTrans;
    public float _speed = 2;
    public float _turnSpeed = 3;
    private NavMeshAgent _agent;
    private Vector3 _desVelocity;
    private CharacterController _charControl;


    void Start()
    {

        this._agent = this.gameObject.GetComponent<NavMeshAgent>();
        this._charControl = this.gameObject.GetComponent<CharacterController>();

        this._agent.destination = this._playerTrans.position;

        return;
    }

    void Update()
    {

        Vector3 lookPos;
        Quaternion targetRot;

        this._agent.destination = this._playerTrans.position;
        this._desVelocity = this._agent.desiredVelocity;

        this._agent.updatePosition = false;
        this._agent.updateRotation = false;

        lookPos = this._playerTrans.position - this.transform.position;
        lookPos.y = 0;
        targetRot = Quaternion.LookRotation(lookPos);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * this._turnSpeed);

        this._charControl.Move(this._desVelocity.normalized * this._speed * Time.deltaTime);

        this._agent.velocity = this._charControl.velocity;

        return;
    }
    
}
