using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variables
    public AudioClip bulletHitSound;


    // The time till the bullet despawns.
    [SerializeField]
    private float despawnTime = 5;
    
    // The tank that shot the bullet.
    [HideInInspector]
    public TankData myShooter;

    // Update is called once per frame
    void Update()
    {
        // Check if our bullets current time has exceeded the despawn time.
        if(despawnTime <= 0)
        {
            // If it has, destroy the bullet.
            Destroy(gameObject);
        }
        // Count up the time the bullet has been alive.
        despawnTime -= Time.deltaTime;
    }

    // If the object with this script touches object as tagged as enemy ship then destroy itself
    void OnCollisionEnter(Collision collision)
    {
        GameObject otherObject = collision.collider.gameObject;

        AudioSource.PlayClipAtPoint(bulletHitSound, transform.position);

        Debug.Log(otherObject + " : " + myShooter.gameObject);
        // Avoid collision with the object that shot the bullet.
        if (otherObject == myShooter.gameObject) { return; }

        TankHealth otherHealth = collision.collider.GetComponent<TankHealth>();

        if (otherHealth)
        {
            otherHealth.TakeDamage(myShooter);
        }
        // Destroy the bullet on collision with another object.
        Destroy(gameObject);
    }
}
