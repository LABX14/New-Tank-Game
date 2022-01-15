using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // Variables
    public TankData data;
    public int score;
    public InputController.InputScheme inputScheme;
    public Spawner spawner;
    public bool isDead = false;
    public int lives;

    public Player(Spawner playerSpawner, float respawnTime, GameObject playerPrefab, InputController.InputScheme playerInputScheme)
    {
        spawner = playerSpawner;
        spawner.spawnDelay = respawnTime;
        spawner.prefabToSpawn = playerPrefab;
        inputScheme = playerInputScheme;
    }
}
