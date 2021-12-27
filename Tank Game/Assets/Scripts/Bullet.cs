using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variables 

    // This will determine the speed of the bullet
    public float bulletSpeed;
    public GameObject myShooter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    // If the object with this script touches object as tagged as enemy ship then destroy itself
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TankData>())
        {
            Destroy(gameObject);
        }
    }
}
