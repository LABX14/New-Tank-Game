using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Variables

    // This tracks the time between each shot
    public float timeBetweenBooms;

    // This adds a count down after a shot
    public float countDown;

    // This tells the player when they can shoot again
    public float timeForNextBoom;

    // This tracks when the player last shot
    public float timeOfLastBoom;

    // Start is called before the first frame update
    void Start()
    {
        // Start our countdown at max 
        countDown = timeBetweenBooms;

        // Set the time that we allowed to boom 
        timeForNextBoom = Time.time + timeBetweenBooms;

        // Set the time it last boomed
        timeOfLastBoom = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeForNextBoom + timeBetweenBooms)
        {
            // Set the time we last boomed
            timeOfLastBoom = Time.time;
        }
    }
}
