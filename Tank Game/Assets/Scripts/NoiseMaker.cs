using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    //Variables
    [Header("Settings")]
    // How loud our movement is.
    public float moveVolume;
    // How loud our turning is.
    public float turnVolume;
    // How loud our shooting is.
    public float shootVolume;           

    [Header("Volume")]
    // How far away the tank can be heard by AI.
    public float volume;
    // How much the volume decreases per second.
    public float decay;     

    // Update is called once per frame
    void Update()
    {
        //Decay Volume
        if (volume > 0)
        {
            volume -= decay * Time.deltaTime;
        }
    }
}
