using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerObstacleAvoidance : MonoBehaviour
{
    // Variables
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;
    
    [Header("Obstacle Avoidance Settings")]
    private int avoidanceStage = 0;
    public float avoidanceTime = 2.0f;
    private float exitTime;

    [Header("Chase and Flee Settings")]
    public Transform target;
    public enum AttackMode { Chase };
    public AttackMode attackMode;

    // Start is called before the first frame update
    void Start()
    {
        // Get components.
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
        data = GetComponent<TankData>();

        GameManager.instance.enemyTanks.Add(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackMode == AttackMode.Chase)
        {
            if (avoidanceStage != 0)
            {
                DoAvoidance();
            }
            else
            {
                DoChase();
            }
        }
    }
    void DoChase()
    {
        motor.RotateTowards(target.position, data.turnSpeed);
        // Check if we can move "data.moveSpeed" units away.
        //    We chose this distance, because that is how far we move in 1 second,
        //    This means, we are looking for collisions "one second in the future."
        if (CanMove(data.moveSpeed))
        {
            motor.Move(data.moveSpeed);
        }
        else
        {
            // Enter obstacle avoidance stage 1
            avoidanceStage = 1;
        }
    }

    void DoAvoidance()
    {
        if (avoidanceStage == 1)
        {
            // Rotate left
            motor.Turn(-1 * data.turnSpeed);

            // If I can now move forward, move to stage 2!
            if (CanMove(data.moveSpeed))
            {
                avoidanceStage = 2;

                // Set the number of seconds we will stay in Stage2
                exitTime = avoidanceTime;
            }

            // Otherwise, we'll do this again next turn!
        }
        else if (avoidanceStage == 2)
        {
            // if we can move forward, do so
            if (CanMove(data.moveSpeed))
            {
                // Subtract from our timer and move
                exitTime -= Time.deltaTime;
                motor.Move(data.moveSpeed);

                // If we have moved long enough, return to chase mode
                if (exitTime <= 0)
                {
                    avoidanceStage = 0;
                }
            }
            else
            {
                // Otherwise, we can't move forward, so back to stage 1
                avoidanceStage = 1;
            }
        }
    }

    // CanMove - checks if I can move "speed" units forward. If so, returns true. If not, returns false.
    public bool CanMove(float speed)
    {
        // Cast a ray forward in the distance that we sent in
        RaycastHit hit;

        // If our raycast hit something...
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed))
        {
            // ... and if what we hit is not the player...
            if (!hit.collider.CompareTag("Player"))
            {
                // ... then we can't move
                return false;
            }
        }
        // otherwise, we can move, so return true
        return true;
    }

}
