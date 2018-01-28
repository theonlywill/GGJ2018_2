using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : MonoBehaviour {

    //public GameObject starslot1;
    //public GameObject starslot2;
    //public GameObject starslot3;

    public GameObject starfill1;
    public GameObject starfill2;
    public GameObject starfill3;

    // Use this for initialization
    void Start ()
    {
        SetStarsToMatchLevelProgress();
	}

    void SetStarsToMatchLevelProgress()
    {
        int numStars = LevelSystem.GetStarsForCurrentLevel();

        starfill1.SetActive(numStars > 0);
        starfill2.SetActive(numStars > 1);
        starfill3.SetActive(numStars > 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleContinue()
    {
        LevelSystem.GoToNextLevel();
    }
}
