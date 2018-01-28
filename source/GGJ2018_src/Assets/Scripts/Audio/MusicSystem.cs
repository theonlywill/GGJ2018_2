using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem 
{
    public MusicPlayer player;
    public MusicPrefabs prefabs;

    static MusicSystem activeInstance;
    public MusicSystem Instance
    {
        get
        {
            if(activeInstance == null)
            {
                activeInstance = new MusicSystem();
                BuildPlayer();
            }
            return activeInstance;
        }
    }

    void BuildPlayer()
    {
        GameObject go = Resources.Load<GameObject>("Audio/PR_MusicPrefabs");
        prefabs = go.GetComponent<MusicPrefabs>();
        GameObject.DontDestroyOnLoad(go);
    }
}
