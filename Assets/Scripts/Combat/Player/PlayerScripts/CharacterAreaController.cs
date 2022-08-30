using System.Collections;
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
    private CharacterMovement movement;
    private PlayerBasicAttack basicAttack;
    private PlayerRoll roll;
    
    // Start is called before the first frame update
    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        basicAttack = GetComponent<PlayerBasicAttack>();
        roll = GetComponent<PlayerRoll>();

    }

    // Update is called once per frame
    private void Update()
    {
        //ability do stuff based on current state
        switch (currentState)
        {
            case State.idle:
                movement.Move();
                roll.Roll();
                basicAttack.BasicAttack();
                break;
            case State.moveing:
                movement.Move();
                roll.Roll();
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

    public void ChangeState(State state)
    {
        currentState = state;
    }

    public State GetState()
    {
        return currentState;
    }
   

}
