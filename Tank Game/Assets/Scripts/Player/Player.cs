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

    // At the player spawnpoints, the all the features of the player will spawn
    // Those are its spawner that will spawn it at a another location and..
    // its respawn time 
    // its character prefab
    public Player(Spawner playerSpawner, float respawnTime, GameObject playerPrefab, InputController.InputScheme playerInputScheme)
    {
        spawner = playerSpawner;
        spawner.spawnDelay = respawnTime;
        spawner.prefabToSpawn = playerPrefab;
        inputScheme = playerInputScheme;
    }

    public void UpdateScore()
    {
        score = data.score;
    }
}
