using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    //Variables
    private NoiseMaker noiseMaker;
    private bool canShoot = true;
    private float shotCoolDown = 0;


    private void Start()
    {
        noiseMaker = GetComponent<NoiseMaker>();
    }

    private void Update()
    {
        if (shotCoolDown > 0)
        {
            canShoot = false;
            shotCoolDown -= Time.deltaTime;
        }
        else if(!canShoot)
        {
            canShoot = true;
        }
    }

    // This controls the shoot function
    public void Shoot(GameObject bulletPrefab, Transform bulletTransform, float speed, float fireRate)
    {
        
        // Check if the tank is able to shoot.
        if (!canShoot) return;

        // The projectile we are shooting.
        GameObject bullet;

        // Create the projectile
        bullet = Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation);
        Debug.Log(bullet.name);

        // Set Bullets shooter.
        bullet.GetComponent<Bullet>().myShooter = GetComponent<TankData>();

        // Launch projectile.
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * speed;

        shotCoolDown = 1/fireRate;

        // Make noise.
        noiseMaker.volume = Mathf.Max(noiseMaker.volume, noiseMaker.shootVolume);
    }
}
