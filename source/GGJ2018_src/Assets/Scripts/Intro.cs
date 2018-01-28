using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public float loadMainMenuAfterXSeconds = 1f;

    float startTime = 0f;
	// Use this for initialization
	void Start ()
    {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Time.time - startTime > loadMainMenuAfterXSeconds)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
	}
}
