﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Controlling player actions and state
/// </summary>
public class CharacterAreaController : MonoBehaviour
{

    //possible player states
    public enum State
    {
        idle,
        moveing,
        basicAttack,
        roll
    }
   
    State currentState = State.idle;

    //Needed components
    private CharacterMovement movement;
    private PlayerBasicAttack basicAttack;
    private PlayerRoll roll;
    public bool canBeHit { get; set; } = true;

    public float pushForce { get; set; } = 0;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //push enemies away
        CharacterController characterController = hit.collider.GetComponent<CharacterController>();
        EnemyController enemyController = hit.gameObject.GetComponent<EnemyController>();
        EnemyActions enemyActions = hit.gameObject.GetComponent<EnemyActions>();

        // no character controller
        if (characterController == null || enemyController == null || enemyActions == null)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }
        if (enemyController.CheckCanBeKnockedBack())
        {
           Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
           enemyActions.MoveOverTime(pushDir, pushForce, 0.2f);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        basicAttack = GetComponent<PlayerBasicAttack>();
        roll = GetComponent<PlayerRoll>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //ability to do stuff based on current state
        
        switch (currentState)
        {
            case State.idle:
                movement.Move();
                basicAttack.BasicAttack();
                break;
            case State.moveing:
                movement.Move();
                basicAttack.BasicAttack();
                break;
            case State.basicAttack:
                basicAttack.BasicAttack();
                break;
            case State.roll:
                
            default:
                break;
        }
        
    }
    private void Update() 
    {
        switch (currentState)
        {
            case State.idle:
                roll.Roll();
                break;
            case State.moveing:
                roll.Roll();
                break;
            case State.basicAttack:
                break;
            case State.roll:

            default:
                break;
        }
    }

    public void ChangeState(State state)
    {
        currentState = state;
    }

    public State GetState()
    {
        return currentState;
    }
    

}
