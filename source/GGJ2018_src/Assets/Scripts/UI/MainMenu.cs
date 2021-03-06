﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SaveSystem.Load();

        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.LogWarning("Forcing to portrait resolution on pc");
            Screen.SetResolution(432, 768, false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        

	}

    public void HandleStartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }

    public void HandleQuitButton()
    {
        Application.Quit();
    }

    public void HandleTutorialButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }
}
