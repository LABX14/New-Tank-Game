using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{

    // Variables
    private Rigidbody rigidbody;
    private TankData data;

    #region Private Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get components.
        rigidbody = GetComponent<Rigidbody>();
        data = GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    #endregion

    #region Public Methods
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

    public void Shoot(GameObject bulletPrefab, Transform bulletTransform, float speed) 
    {
        // The projectile we are shooting.
        Rigidbody bullet;

        // Create the projectile
        bullet = Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation).GetComponent<Rigidbody>();

        // Launch projectile.
        bullet.velocity = transform.forward * speed*Time.deltaTime;

    }
    #endregion
}
