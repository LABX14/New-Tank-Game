using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
    // Variables
    public float speedModifier;
    public float healthModifier;
    public float maxHealthModifier;
    public float fireRateModifier;

    public float duration;
    public bool isPermanent;


    // This is to change the player's max health 
    public void OnActivate(TankData target)
    {
        target.moveSpeed += speedModifier;
        
        if(healthModifier != 0 || maxHealthModifier != 0)
        {
            TankHealth health = target.GetComponent<TankHealth>();
            health.HealDamage(healthModifier);
            health.IncreaseMaxHelth(maxHealthModifier);
        }

        target.fireRate += fireRateModifier;
    }

    // This will change the player's max health back to normal after the power up deactivates
    public void OnDeactivate(TankData target)
    {
        target.moveSpeed -= speedModifier;

        if (healthModifier != 0 || maxHealthModifier != 0)
        {
            TankHealth health = target.GetComponent<TankHealth>();
            health.HealDamage(-healthModifier);
            health.IncreaseMaxHelth(-maxHealthModifier);
        }

        target.fireRate -= fireRateModifier;
    }
}
