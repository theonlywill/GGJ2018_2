using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public LevelInfo levelInfo;
    public Text label;

    public Sprite filledStarSprit;
    public Sprite emptyStarSprite;

    public Image star1;
    public Image star2;
    public Image star3;

	// Use this for initialization
	void Start ()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
	}

    void OnClicked()
    {
        if(levelInfo)
        {
            LevelSystem.LoadLevel(levelInfo);
        }
        else
        {
            Debug.LogError("NO level info on this button???");
        }
    }
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public void UpdateStars()
    {
        // todo: grab from save data
        int numStars = LevelSystem.GetNumStars(levelInfo.levelNumber);
        star1.sprite = numStars > 0 ? filledStarSprit : emptyStarSprite;
        star2.sprite = numStars > 1 ? filledStarSprit : emptyStarSprite;
        star3.sprite = numStars > 2 ? filledStarSprit : emptyStarSprite;
    }
}
