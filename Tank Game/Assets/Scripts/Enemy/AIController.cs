using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Variables
    // The Tank Motor component.
    private TankMotor motor;

    // The Tank Shooter component.
    private TankShooter shooter;

    // The Tank Data component.
    private TankData data;

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
    }
}
