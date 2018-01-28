using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindow : MonoBehaviour {

    public GameObject clickBlocker;
    public GameObject canvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClicked()
    {
        clickBlocker.SetActive(false);
        canvas.SetActive(false);
    }
}
