using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Variables
    
    // Input types.
    private enum InputScheme { WASD, arrowKeys };
    
    // The currently selected input type.
    [SerializeField]
    private InputScheme input = InputScheme.WASD;

    // The Tank Motor component.
    private TankMotor motor;

    // The Tank Data component.
    private TankData data;

    private void Start()
    {
        // Get components.
        motor = GetComponent<TankMotor>();
        data = GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (input)
        {
            case InputScheme.arrowKeys:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    motor.Turn(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    motor.Turn(-data.turnSpeed);
                }
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    motor.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed);
                }
                break;
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W))
                {
                    motor.Move(data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    motor.Move(-data.moveSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    motor.Turn(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    motor.Turn(-data.turnSpeed);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    motor.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed);
                }
                break;
        }
    }
}