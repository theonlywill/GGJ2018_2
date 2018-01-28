using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    [System.Serializable]
    public class LevelSaveData
    {
        public int level = 0;
        public int stars = 0;
    }
    public List<LevelSaveData> levelSaveData = new List<LevelSaveData>();
}