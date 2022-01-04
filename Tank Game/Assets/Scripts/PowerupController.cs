using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{

    // Variables
    public List<Powerup> powerups;
    private List<Powerup> expiredPowerups;
    private TankData data;

    // Start is called before the first frame update
    void Start()
    {
        powerups = new List<Powerup>();
        expiredPowerups = new List<Powerup>();
        data = GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Powerup powerup in powerups)
        {
            powerup.duration -= Time.deltaTime;

            if(powerup.duration <= 0)
            {
                expiredPowerups.Add(powerup);
            }
        }

        foreach (Powerup powerup in expiredPowerups)
        {
            powerup.OnDeactivate(data);
            powerups.Remove(powerup);
        }

        expiredPowerups.Clear();
    }

    public void Add (Powerup powerup)
    {
        powerup.OnActivate(data);
        if (!powerup.isPermanent)
        {
            powerups.Add(powerup);
        }
    }
}
