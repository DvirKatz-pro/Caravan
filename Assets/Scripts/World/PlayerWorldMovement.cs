using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Handles 2D player movement
/// </summary>
public class PlayerWorldMovement : MonoBehaviour
{

    public float movementSpeed = 1.0f;

    private Rigidbody2D rb;
    private bool isRight = true;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalInput)
    {
        Vector2 currentPos = rb.position;
        Vector2 inputVector = new Vector2(horizontalInput, 0);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        if (horizontalInput > 0 && !isRight)
        {
            isRight = true;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (horizontalInput < 0 && isRight)
        {
            isRight = false;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }


    }
}
