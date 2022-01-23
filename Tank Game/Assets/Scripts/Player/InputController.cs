using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Variables

    // Input types.
    public enum InputScheme { WASD, arrowKeys };

    // The currently selected input type.
    [SerializeField]
    private InputScheme input = InputScheme.WASD;

    // The Tank Motor component.
    private TankMotor motor;

    // The Tank Shooter component.
    private TankShooter shooter;

    // The Tank Data component.
    private TankData data;

    public int playerIndex;

    private void Start()
    {
        // Get components.
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
        data = GetComponent<TankData>();

        // Give input scheme to the players when they spawn in based on which game mode they will do 
        if (GameManager.instance.players[0].data == null)
        {
            GameManager.instance.players[0].data = data;
            playerIndex = 0;
            GameManager.instance.player1Camera.transform.SetParent(transform, false);
        }
        else if (GameManager.instance.isMultiplayer && GameManager.instance.players[1].data == null)
        {
            GameManager.instance.players[1].data = data;
            playerIndex = 1;
            GameManager.instance.player2Camera.transform.SetParent(transform, false);
        }
        else
        {
            Destroy(gameObject);
        }

        input = GameManager.instance.players[playerIndex].inputScheme;
        GameManager.instance.players[playerIndex].isDead = false;
        data.score = GameManager.instance.players[playerIndex].score;
    }

    // Update is called once per frame
    void Update()
    {
        switch (input)
        {
            // These are the directions that the player will move when these arrow keys are pressed 
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
                    shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);
                }
                break;

                // These are how the players wwill move the certain keys are pressed
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
                    shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);
                }
                break;
        }
    }

    // When it is destroyed, set it the player the player is alive
    private void OnDestroy()
    {
        GameManager.instance.players[playerIndex].score = data.score;
        if (playerIndex == 0)
        {
            GameManager.instance.player1Camera.transform.SetParent(GameManager.instance.transform, false);
            GameManager.instance.player1Camera.enabled = true;

        }
        else
        {
            GameManager.instance.player2Camera.transform.SetParent(GameManager.instance.transform, false);
            GameManager.instance.player2Camera.enabled = true;
        }
        GameManager.instance.players[playerIndex].isDead = true;
    }
}
