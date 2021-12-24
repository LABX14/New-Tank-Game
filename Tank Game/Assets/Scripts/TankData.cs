using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    // Variable

    // Move speed in meters per second.
    public float moveSpeed = 3;

    // Turn speed in degrees per second.
    public float turnSpeed = 180;

    public float bulletSpeed = 10;

    public GameObject bulletPrefab;

    public Transform bulletTransform;
}
