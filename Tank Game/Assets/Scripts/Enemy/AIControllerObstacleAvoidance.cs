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
    public float fieldOfView = 45.0f;
    public float hearingRadius = 5f;

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
        if (CanSee(target.gameObject) || CanHear(target.gameObject))
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

    public bool CanSee(GameObject target)
    {
        // We use the location of our target in a number of calculations - store it in a variable for easy access.
        Vector3 targetPosition = target.transform.position;

        // Find the vector from the agent to the target
        // We do this by subtracting "destination minus origin", so that "origin plus vector equals destination."
        Vector3 agentToTargetVector = targetPosition - transform.position;

        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, transform.forward);

        // if that angle is less than our field of view
        if (angleToTarget < fieldOfView)
        {
            // Create a variable to hold a ray from our position to the target
            Ray rayToTarget = new Ray();

            // Set the origin of the ray to our position, and the direction to the vector to the target
            rayToTarget.origin = transform.position;
            rayToTarget.direction = agentToTargetVector;

            // Create a variable to hold information about anything the ray collides with
            RaycastHit hitInfo;

            // Cast our ray for infinity in the direciton of our ray.
            //    -- If we hit something...
            if (Physics.Raycast(rayToTarget, out hitInfo, Mathf.Infinity))
            {
                // ... and that something is our target 
                if (hitInfo.collider.gameObject == target)
                {
                    // return true 
                    //    -- note that this will exit out of the function, so anything after this functions like an else
                    return true;
                }
            }
        }
        // return false
        //   -- note that because we returned true when we determined we could see the target, 
        //      this will only run if we hit nothing or if we hit something that is not our target.
        return false;
    }

    public bool CanHear(GameObject target)
    {
        //Check for a noise maker
        NoiseMaker targetNoiseMaker = target.GetComponent<NoiseMaker>();
        if (targetNoiseMaker == null)
        {
            return false;
        }

        //Check if we can hear them
        if (Vector3.Distance(target.transform.position, transform.position) <= targetNoiseMaker.volume + hearingRadius)
        {
            return true;
        }

        //If we can't, return false
        return false;
    }

}
