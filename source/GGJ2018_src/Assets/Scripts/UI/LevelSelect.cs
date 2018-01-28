using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{

    public GameObject levelButtonTemplate;
    public GameObject buttonList;

	// Use this for initialization
	void Start ()
    {
        BuildLevelList();
	}

    void BuildLevelList()
    {
        //hide our template
        levelButtonTemplate.SetActive(false);
        List<LevelInfo> levels = new List<LevelInfo>( Resources.LoadAll<LevelInfo>("Levels"));
        levels.Sort(SortByNumber);

        for (int i = 0; i < levels.Count; i++)
        {
            BuildLevelButton(levels[i]);
        }
    }

    private static int SortByNumber(LevelInfo a, LevelInfo b)
    {
        return a.levelNumber.CompareTo(b.levelNumber);
    }

    void BuildLevelButton(LevelInfo i_level)
    {
        GameObject goNewButton = GameObject.Instantiate<GameObject>(levelButtonTemplate, buttonList.transform, false);
        goNewButton.SetActive(true);
        LevelButton button = goNewButton.GetComponent<LevelButton>();
        button.levelInfo = i_level;
        button.label.text = i_level.levelNumber.ToString();
        button.UpdateStars();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleBackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
