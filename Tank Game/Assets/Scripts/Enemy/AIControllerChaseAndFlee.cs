using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerChaseAndFlee : MonoBehaviour
{
    // Variables
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;
    
    [Header("Chase and Flee Settings")]
    public Transform target;
    public enum AttackMode { Chase, Flee };
    public AttackMode attackMode;
    public float fleeDistance = 1.0f;

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
        switch (attackMode)
        {
            case AttackMode.Chase:
            shooter.Shoot(data.bulletPrefab, data.bulletTransform, data.bulletSpeed, data.fireRate);
            // Rotate towards the target
            motor.RotateTowards(target.position, data.turnSpeed);
            // Move forward
            motor.Move(data.moveSpeed);
            break;

            case AttackMode.Flee:
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
            motor.Move(data.moveSpeed);
            break;
        }
    }
}
