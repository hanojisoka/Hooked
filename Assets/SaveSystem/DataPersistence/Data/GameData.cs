using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Level;
    public int FishCount;

    public GameData()
    {
        this.Level = 0;
        this.FishCount = 0;
    }
}
