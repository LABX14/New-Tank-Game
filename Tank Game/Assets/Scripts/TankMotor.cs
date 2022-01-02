using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{

    // Variables
    private Rigidbody rigidbody;
    private TankData data;

    // Start is called before the first frame update
    void Start()
    {
        // Get components.
        rigidbody = GetComponent<Rigidbody>();
    }

    // Moves the tank forward and backwards.
    public void Move(float speed)
    {

        // The tanks movement data.
        Vector3 movementVector;
        // Get the direction facing infront of the tank.
        movementVector = transform.forward;

        // Apply the speed to the forward direction.
        movementVector *= speed;

        // Move the tank forwards
        rigidbody.MovePosition(rigidbody.position + (movementVector * Time.deltaTime));

        // Test code to move forward 
        //characterController.SimpleMove(movement.forward * speed);

    }

    public void Turn(float speed) 
    {
        // The tanks rotation data.
        Vector3 rotationVector;

        // Get the direction facing infront of the tank.
        rotationVector = Vector3.up;

        // Apply the speed to the forward direction.
        rotationVector *= speed * Time.deltaTime;

        // Turn the tank.
        transform.Rotate(rotationVector, Space.Self);
    }
}
