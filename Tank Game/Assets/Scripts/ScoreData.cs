using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScoreData : IComparable<ScoreData>
{
    // Variables
    public float score;
    public string name;

    // This will track the player's name and score
    public ScoreData(string name, float score)
    {
        this.name = name;
        this.score = score;
    }

    // this will track the score data
    public int CompareTo(ScoreData other)
    {
        if (other == null)
        {
            return 1;
        }
        if (this.score > other.score)
        {
            return 1;
        }
        if (this.score < other.score)
        {
            return -1;
        }
        return 0;
    }
}
