using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour
{
    public Transform[] waypoints;
    public float closeEnough = 1.0f;
    public TankMotor motor;
    public TankData data;
    private int currentWaypoint = 0;
    private Transform tf;

    void Awake()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}