using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level", menuName ="Level Info", order = 42)]
public class LevelInfo : ScriptableObject
{
    public int levelNumber = 1;
    //public UnityEngine.SceneManagement.Scene scene;

    public int numFuelPacks = 0;
    
    public int numAttract = 0;
    public int numRepel = 0;
    public int numDelay = 0;
    public int numShield = 0;
    public int numMissile = 0;
    
}
