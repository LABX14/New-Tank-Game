using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    // Variables
    public PowerupController powerupController;
    [SerializeField]
    private Powerup cheatPowerup;

    // Start is called before the first frame update
    void Start()
    {
        powerupController = GetComponent<PowerupController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha4) && Input.GetKey(KeyCode.Alpha6) && Input.GetKeyDown(KeyCode.Alpha7))
        {
            powerupController.Add(cheatPowerup);
        }
    }
}
