using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerWaypoints : MonoBehaviour
{
    // Variables

    // Tank Components
    // The Tank Motor component.
    private TankMotor motor;

    // The Tank Shooter component.
    private TankShooter shooter;

    // The Tank Data component.
    private TankData data;

    [Header("Waypoint Settings")]
    public Transform[] waypoints;
    private int currentWaypoint = 0;
    public float closeEnough = 1.0f;
    private enum LoopType { Stop, Loop, PingPong };
    [SerializeField]
    private LoopType loopType;
    private bool isPatrolForward = true;

    // Start is called before the first frame update
    private void Start()
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
        shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);

        // If we are close to the waypoint,
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - transform.position) < (closeEnough * closeEnough))
        {
            switch (loopType)
            {
                case LoopType.Stop:
                // Advance to the next waypoint, if we are still in range
                if (currentWaypoint < waypoints.Length - 1)
                {
                    currentWaypoint++;
                }
                break;

                case LoopType.Loop:
                // Advance to the next waypoint, if we are still in range
                if (currentWaypoint < waypoints.Length - 1)
                {
                    currentWaypoint++;
                }
                else
                {
                    currentWaypoint = 0;
                }
                break;

                case LoopType.PingPong:           
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
                break;  
            }
        }
        else
        {
            if (motor.RotateTowards(waypoints[currentWaypoint].position, data.turnSpeed))
            {
                Debug.Log("Rotating towards " + waypoints[currentWaypoint].name);
                // Do nothing!
            }
            else
            {
                // Move forward
                motor.Move(data.moveSpeed);
            }
        }
    }
}
