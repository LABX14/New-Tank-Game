using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MapGenerator : MonoBehaviour
{
    // Variables

    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private float roomWidth = 50.0f;
    [SerializeField]
    private float roomHeight = 50.0f;
    [SerializeField]
    private GameObject[] gridPrefabs;

    private Room[,] grid;

    public int mapSeed;

    // What seed to use for the map.
    public enum MapType { mapOfTheDay, random, custom };     
    public MapType mapType;


    // Start is called before the first frame update
    void Start()
    {
        //Set the Seed
        switch (mapType)
        {
            case MapType.mapOfTheDay:
                mapSeed = DateToInt(DateTime.Today);
                break;

            case MapType.random:
                mapSeed = DateToInt(DateTime.Now);
                break;

            case MapType.custom:
                //Set the seed in the inspector
                break;
        }

        // Set our seed
        UnityEngine.Random.InitState(mapSeed);

        // Generate Grid
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        // Clear out the grid
        grid = new Room[rows, cols];

        // For each grid row...
        for (int i = 0; i < rows; i++)
        {
            // for each column in that row
            for (int j = 0; j < cols; j++)
            { 
                // Figure out the location. 
                float xPosition = roomWidth * j;
                float zPosition = roomHeight * i;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                // Create a new grid at the appropriate location
                GameObject tempRoomObj = Instantiate(gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)], newPosition, Quaternion.identity) as GameObject;

                // Set its parent
                tempRoomObj.transform.parent = transform;

                // Give it a meaningful name
                tempRoomObj.name = "Room_" + j + "," + i;

                // Get the room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // Open the doors
                // If we are on the bottom row, open the north door
                if (i == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (i == rows - 1)
                {
                    // Otherwise, if we are on the top row, open the south door
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    // Otherwise, we are in the middle, so open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                // If we are on the first column, open the east door
                if (j == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (j == cols - 1)
                {
                    // Otherwise, if we are on the last column row, open the west door
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    // Otherwise, we are in the middle, so open both doors
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                // Save it to the grid array
                grid[i, j] = tempRoom;
            }
        }
    }

    public int DateToInt(DateTime dateToUse)
    {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
