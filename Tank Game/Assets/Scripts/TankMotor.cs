using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TankMotor : MonoBehaviour
{

    // Variables
    private CharacterController characterController;
    private Rigidbody rigidbody;
    private TankData data;
    private Transform tf;

    public Transform[] waypoints;
    public TankMotor motor;



    private int currentWaypoint = 0;
    public float closeEnough = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Get components.
        rigidbody = GetComponent<Rigidbody>();
        // characterController = GetComponent<CharacterController>();
        data = GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Moves the tank forward and backwards.
    public void Move(float speed)
    {

        // The tanks movement data.
        Vector3 movementVector;
        Debug.Log("I am moving!");
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

        Debug.Log("I am turning!");
    }
    
    // This controls the shoot function
    public void Shoot(GameObject bulletPrefab, Transform bulletTransform, float speed) 
    {
        // The projectile we are shooting.
        Rigidbody bullet;

        // Create the projectile
        bullet = Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation).GetComponent<Rigidbody>();

        // Launch projectile.
        bullet.velocity = transform.forward * speed*Time.deltaTime;

    }

    public bool RotateTowards(Vector3 target, float speed)
    {
        if (RotateTowards(waypoints[currentWaypoint].position, data.turnSpeed))
        {
            // Variables
            Vector3 vectorToTarget;

            // The vector to our target is the DIFFERENCE
            //      between the target position and our position.
            vectorToTarget = target - tf.position;

            // Find the Quaternion that looks down that vector
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
        }
        else
        {
            // Move forward
            motor.Move(data.moveSpeed);
        }
        // If we are close to the waypoint,
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < closeEnough)
        {
            // Advance to the next waypoint
            currentWaypoint++;
        }
        return false;
    }
}
