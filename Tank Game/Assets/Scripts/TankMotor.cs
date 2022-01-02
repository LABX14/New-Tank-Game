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

    // RotateTowards (Target) - rotates towards the target (if possible).
    // If we rotate, then returns true. If we can't rotate (because we are already facing the target) return false.
    public bool RotateTowards(Vector3 target, float speed)
    {
        Vector3 vectorToTarget;

        // The vector to our target is the DIFFERENCE between the target position and our position.
        //   How would our position need to be different to reach the target? "Difference" is subtraction!
        vectorToTarget = target - transform.position;

        // Find the Quaternion that looks down that vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);


        Debug.Log("Rotations: " + targetRotation + ":" + transform.rotation);
        // If that is the direction we are already looking, we don't need to turn!
        if (targetRotation == transform.rotation)
        {
            return false;
        }

        // Otherwise:
        // Change our rotation so that we are closer to our target rotation, but never turn faster than our Turn Speed
        //   Note that we use Time.deltaTime because we want to turn in "Degrees per Second" not "Degrees per Framedraw"
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);


        // We rotated, so return true
        return true;
    }

}
