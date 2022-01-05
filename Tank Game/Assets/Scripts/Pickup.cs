using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Variables
    [SerializeField]
    private Powerup powerup;

    [SerializeField]
    private AudioClip feedback;

    public void OnTriggerEnter(Collider other)
    {
        // variable to store other object's PowerupController - if it has one
        PowerupController powerupController = other.GetComponent<PowerupController>();

        // If the other object has a PowerupController
        if (powerupController != null)
        {
            // Add the powerup
            powerupController.Add(powerup);

            // Play Feedback (if it is set)
            if (feedback != null)
            {
                AudioSource.PlayClipAtPoint(feedback, transform.position, 1.0f);
            }

            // Destroy this pickup
            Destroy(gameObject);
        }
    }
}
