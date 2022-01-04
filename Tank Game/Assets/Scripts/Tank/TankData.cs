using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    // Variable

    [Header("Movement Settings")]
    // Move speed in meters per second.
    public float moveSpeed = 3;

    // Turn speed in degrees per second.
    public float turnSpeed = 180;

    [Header("Bullet Settings")]

    public float bulletSpeed = 10;

    public GameObject bulletPrefab;

    public Transform bulletTransform;

    public float bulletDamage = 1;

    // Time till next shot can be fired in seconds.
    public float fireRate = 1;

    [Header("Health Settings")]

    public float tankHP = 3;

    [Header("Score Settings")]

    public int tankPointValue = 1;

    public int score = 0;

}