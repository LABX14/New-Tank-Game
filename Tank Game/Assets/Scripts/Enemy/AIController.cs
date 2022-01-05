using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Variables

    // Tank Components

    // The Tank Motor component.
    private TankMotor motor;

    // The Tank Shooter component.
    private TankShooter shooter;

    // The Tank Data component.
    private TankData data;

    // The Tank Health component.
    private TankHealth health;

    public enum AIPersonality { Fleeing, Patrolling, Hunting, Wandering};
    public AIPersonality personality = AIPersonality.Hunting;

    [Header("Obstacle Avoidance Settings")]
    public float avoidanceTime = 2.0f;
    private int avoidanceStage = 0;
    private float exitTime;

    [Header("Chase and Flee Settings")]
    public Transform target;
    public float fleeDistance = 1.0f;
    public float chaseTime = 10f;
    public float fleeTime = 30f;

    [Header("Waypoint Settings")]
    public Transform[] waypoints;
    private int currentWaypoint = 0;
    public float closeEnough = 1.0f;
    private bool isPatrolForward = true;

    [Header("State Machine and Senses Settings")]
    public float fieldOfView = 45.0f;
    public float hearingRadius = 5f;
    public float stateEnterTime;
    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Default };
    public AIState aiState = AIState.Chase;
    public float restingHealRate;

    // Start is called before the first frame update
    void Start()
    {
        // Get components.
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
        data = GetComponent<TankData>();
        health = GetComponent<TankHealth>();

        GameManager.instance.enemyTanks.Add(data);

        target = GameManager.instance.players[0].transform;

        if((personality == AIPersonality.Patrolling || personality == AIPersonality.Wandering) && waypoints.Length == 0)
        {
            //Find Waypoints.
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (personality)
        {
            case AIPersonality.Fleeing:
                if (aiState == AIState.Flee)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoFlee();
                    }

                    // Check for Transitions
                    if (Time.time >= stateEnterTime + fleeTime)
                    {
                        ChangeState(AIState.CheckForFlee);
                    }
                }
                else if (aiState == AIState.CheckForFlee)
                {
                    // Perform Behaviors
                    CheckForFlee();

                    // Check for Transitions
                    if (CanHear(target.gameObject) || CanSee(target.gameObject))
                    {
                        ChangeState(AIState.Flee);
                    }
                    else
                    {
                        ChangeState(AIState.Default);
                    }
                }
                else if (aiState == AIState.Default)
                {
                    // Perform Behaviors
                    DoRest();

                    // Check for Transitions
                    if (CanHear(target.gameObject) || CanSee(target.gameObject))
                    {
                        ChangeState(AIState.Flee);
                    }
                }
                else
                {
                    ChangeState(AIState.Default);
                }
                break;

            case AIPersonality.Patrolling:
                if (aiState == AIState.Default)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoPatrol();
                    }
                    // Check for Transitions
                    if (CanSee(target.gameObject) || CanHear(target.gameObject))
                    {
                        ChangeState(AIState.Chase);
                    }
                }
                else if (aiState == AIState.Chase)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoChase();
                    }

                    // Check for Transitions
                    if (Time.time >= stateEnterTime + chaseTime)
                    {
                        ChangeState(AIState.Default);
                    }
                    else if (CanSee(target.gameObject))
                    {
                        ChangeState(AIState.ChaseAndFire);
                    }
                }
                else if (aiState == AIState.ChaseAndFire)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoChase();
                        shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);
                    }
                    // Check for Transitions
                    if (Time.time >= stateEnterTime + chaseTime)
                    {
                        ChangeState(AIState.Default);
                    }
                    else if (!CanSee(target.gameObject) && CanHear(target.gameObject))
                    {
                        ChangeState(AIState.Chase);
                    }
                }
                else
                {
                    ChangeState(AIState.Default);
                }
                break;

            case AIPersonality.Hunting:
                if (aiState == AIState.Chase)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoChase();
                    }

                    // Check for Transitions
                    if (health.GetCurrentHP() < data.tankHP * 0.5f)
                    {
                        ChangeState(AIState.CheckForFlee);
                    }
                    else if (CanSee(target.gameObject))
                    {
                        ChangeState(AIState.ChaseAndFire);
                    }
                }
                else if (aiState == AIState.ChaseAndFire)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoChase();
                        shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);
                    }
                    // Check for Transitions
                    if (health.GetCurrentHP() < data.tankHP * 0.5f)
                    {
                        ChangeState(AIState.CheckForFlee);
                    }
                    else if (!CanSee(target.gameObject))
                    {
                        ChangeState(AIState.Chase);
                    }
                }
                else if (aiState == AIState.Flee)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoFlee();
                    }

                    // Check for Transitions
                    if (Time.time >= stateEnterTime + fleeTime)
                    {
                        ChangeState(AIState.CheckForFlee);
                    }
                }
                else if (aiState == AIState.CheckForFlee)
                {
                    // Perform Behaviors
                    CheckForFlee();

                    // Check for Transitions
                    if (CanHear(target.gameObject) || CanSee(target.gameObject))
                    {
                        ChangeState(AIState.Flee);
                    }
                    else
                    {
                        ChangeState(AIState.Default);
                    }
                }
                else if (aiState == AIState.Default)
                {
                    // Perform Behaviors
                    DoRest();

                    // Check for Transitions
                    if (health.GetCurrentHP() >= data.tankHP)
                    {
                        ChangeState(AIState.Chase);
                    }
                    else if (CanHear(target.gameObject) || CanSee(target.gameObject))
                    {
                        ChangeState(AIState.Flee);
                    }
                }
                break;

            case AIPersonality.Wandering:
                if (aiState == AIState.Default)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoWaypoints();
                    }

                    // Check for Transitions
                    if (CanSee(target.gameObject) || CanHear(target.gameObject))
                    {
                        ChangeState(AIState.ChaseAndFire);
                    }
                }
                else if (aiState == AIState.ChaseAndFire)
                {
                    // Perform Behaviors
                    if (avoidanceStage != 0)
                    {
                        DoAvoidance();
                    }
                    else
                    {
                        DoWaypoints();
                    }
                    shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);

                    // Check for Transitions
                    if (!CanSee(target.gameObject) && !CanHear(target.gameObject))
                    {
                        ChangeState(AIState.Default);
                    }
                }
                else
                {
                    ChangeState(AIState.Default);
                }
                break;
        }
    }

    public void ChangeState(AIState newState)
    {

        // Change our state
        aiState = newState;

        // save the time we changed states
        stateEnterTime = Time.time;
    }

    public void DoRest()
    {
        // Increase our health.
        health.HealDamage(restingHealRate * Time.deltaTime);
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

    void DoChase()
    {
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

    void DoFlee()
    {
        // The vector from ai to target is target position minus our position.
        Vector3 vectorToTarget = target.position - transform.position;

        // We can flip this vector by -1 to get a vector AWAY from our target
        Vector3 vectorAwayFromTarget = -1 * vectorToTarget;

        // Now, we can normalize that vector to give it a magnitude of 1
        vectorAwayFromTarget.Normalize();

        // A normalized vector can be multiplied by a length to make a vector of that length.
        vectorAwayFromTarget *= fleeDistance;

        // We can find the position in space we want to move to by adding our vector away from our AI to our AI's position.
        //     This gives us a point that is "that vector away" from our current position.
        Vector3 fleePosition = vectorAwayFromTarget + transform.position;
        motor.RotateTowards(fleePosition, data.turnSpeed);
       
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

    void DoPatrol()
    {
        // If we are close to the waypoint,
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - transform.position) < (closeEnough * closeEnough))
        {
            if (isPatrolForward)
            {
                // Advance to the next waypoint, if we are still in range
                if (currentWaypoint < waypoints.Length - 1)
                {
                    currentWaypoint++;
                }
                else
                {
                    //Otherwise reverse direction and decrement our current waypoint
                    isPatrolForward = false;
                    currentWaypoint--;
                }
            }
            else
            {
                // Advance to the next waypoint, if we are still in range
                if (currentWaypoint > 0)
                {
                    currentWaypoint--;
                }
                else
                {
                    //Otherwise reverse direction and increment our current waypoint
                    isPatrolForward = true;
                    currentWaypoint++;
                }
            }
        }
        else
        {
            if (motor.RotateTowards(waypoints[currentWaypoint].position, data.turnSpeed))
            {
                // Do nothing!
            }
            else
            {
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
    }

    void DoWaypoints()
    {
        // If we are close to the waypoint,
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - transform.position) < (closeEnough * closeEnough))
        {
            // Advance to the next waypoint, if we are still in range
            if (currentWaypoint < waypoints.Length - 1)
            {
                currentWaypoint++;
            }
            else
            {
                currentWaypoint = 0;
            }
        }
        else
        {
            if (motor.RotateTowards(waypoints[currentWaypoint].position, data.turnSpeed))
            {
                // Do nothing!
            }
            else
            {
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
    }

    // Runs before the tank begins fleeing
    public void CheckForFlee()
    {
        
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

    private void OnDestroy()
    {
        GameManager.instance.enemyTanks.Remove(data);
    }

}
