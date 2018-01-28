using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindow : MonoBehaviour {

    public GameObject clickBlocker;
    public GameObject canvas;

    public List<Dialog> dialogs = new List<Dialog>();

    [System.Serializable]
    public class Dialog
    {
        public GameObject window;
        public AudioClip audioClip;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClicked()
    {
        bool closedDialog = false;
        for(int i = 0; i < dialogs.Count; i++)
        {
            if(dialogs[i].window.activeSelf)
            {
                dialogs[i].window.SetActive(false);
                closedDialog = true;
                AudioSource audioSource = GetComponent<AudioSource>();
                if(audioSource && dialogs[i].audioClip)
                {
                    audioSource.PlayOneShot(dialogs[i].audioClip);
                }

                break;
            }
        }

        if (!closedDialog)
        {
            // we closed our last dialog
            AudioSource a = GetComponent<AudioSource>();
            a.Stop();

            canvas.SetActive(false);
        }
    }
}
