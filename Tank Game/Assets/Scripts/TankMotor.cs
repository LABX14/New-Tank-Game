using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{

    // Variables
    private Rigidbody rigidbody;

    #region Private Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get components.
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // Debug movement, W for forward S for backward
        if (Input.GetKey(KeyCode.W)) Move(5);
        if (Input.GetKey(KeyCode.S)) Move(-5);
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
        movementVector = movementVector * speed;

        // Move the tank forwards
        rigidbody.MovePosition(rigidbody.position + (movementVector * Time.deltaTime));
    }
    #endregion
}
