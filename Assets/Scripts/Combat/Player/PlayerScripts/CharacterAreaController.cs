using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //possible face directions
    public enum Directions
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
    Dictionary<Directions, float> angleMap = new Dictionary<Directions, float>();
    State currentState = State.idle;
    private CharacterMovement movement;
    private PlayerBasicAttack basicAttack;
    private PlayerRoll roll;

    

    Directions currentDirection = Directions.North;

    
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        if (movement == null)
        {
            Debug.LogError("CharacterMovement not found");
        }
        basicAttack = GetComponent<PlayerBasicAttack>();
        if (basicAttack == null)
        {
            Debug.LogError("CharacterMovement not found");
        }
        roll = GetComponent<PlayerRoll>();
        if (roll == null)
        {
            Debug.LogError("CharacterMovement not found");
        }

        //add angles corresponding to a direction
        /*
        angleMap.Add(Directions.North, 315);
        angleMap.Add(Directions.NorthEast, 0);
        angleMap.Add(Directions.East, 45);
        angleMap.Add(Directions.SouthEast, 90);
        angleMap.Add(Directions.South, 135);
        angleMap.Add(Directions.SouthWest, 180);
        angleMap.Add(Directions.West, 225);
        angleMap.Add(Directions.NorthWest, 270);
        */

        angleMap.Add(Directions.North, 45);
        angleMap.Add(Directions.NorthEast, 90);
        angleMap.Add(Directions.East, 135);
        angleMap.Add(Directions.SouthEast, 180);
        angleMap.Add(Directions.South, 225);
        angleMap.Add(Directions.SouthWest, 270);
        angleMap.Add(Directions.West, 315);
        angleMap.Add(Directions.NorthWest, 0);
        

      


    }

    // Update is called once per frame
    void Update()
    {
        //ability do stuff based on current state
        switch (currentState)
        {
            case State.idle:
                movement.move();
                roll.roll();
                basicAttack.handleBasicAttack();
                break;
            case State.moveing:
                movement.move();
                roll.roll();
                basicAttack.handleBasicAttack();
                break;
            case State.basicAttack:
                basicAttack.handleBasicAttack();
                break;
            case State.roll:
                
            default:
                break;
        }
    }
    /*
     * change a players state
     */
    public void changeState(State state)
    {
        currentState = state;
    }

    public State getState()
    {
        return currentState;
    }
    /**
     * given a direction rotate to that direction
     */
    public void setDirection(Directions heading)
    {
        transform.localEulerAngles = new Vector3(angleMap[heading], 90, -90);
        currentDirection = heading;
    }
    /**
   * given an angle return a direction
   */

    public Directions getDirection(float angle)
    {
        float step = 360 / 8;
        float halfStep = step / 2;

        angle += halfStep;

        if (angle < 0)
        {
            angle += 360.0f;
        }
        float stepCount = angle / step;

        int stepCountInt = Mathf.FloorToInt(stepCount);


        return (Directions)stepCountInt;
    }
    public Directions getCurrentDirection()
    {
        return currentDirection;
    }
}
