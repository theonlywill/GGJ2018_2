using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem
{
    public static LevelInfo currentLevel;


    public static void LoadLevel(LevelInfo i_level)
    {
        currentLevel = i_level;
        SceneManager.LoadScene(i_level.levelNumber.ToString());
    }

    public static int GetNumStars(int i_level)
    {
        if(SaveSystem.currentData != null)
        {
            for(int i = 0; i < SaveSystem.currentData.levelSaveData.Count; i++)
            {
                if(SaveSystem.currentData.levelSaveData[i].level == i_level)
                {
                    return SaveSystem.currentData.levelSaveData[i].stars;
                }
            }
        }
        return 0;
    }

    public static int GetStarsForCurrentLevel()
    {
        int currentLevelNum = GetCurrentLevelNum();



        return GetNumStars(currentLevelNum);
    }

    public static void GoToNextLevel()
    {

        int currentLevelNum = GetCurrentLevelNum();

        // is there a next level?
        int newLevelNum = currentLevelNum + 1;

        List<LevelInfo> levels = new List<LevelInfo>(Resources.LoadAll<LevelInfo>("Levels"));
        bool foundlevel = false;
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].levelNumber == newLevelNum)
            {
                string sceneNameToLoad = levels[i].levelNumber.ToString();
                Scene newScene = SceneManager.GetSceneByName(sceneNameToLoad);
                if (newScene != null)
                {
                    SceneManager.LoadScene(sceneNameToLoad);
                }
                else
                {
                    Debug.LogError("Scene with name " + sceneNameToLoad + " is not in build settings!!!");
                    SceneManager.LoadScene("LevelSelect");
                }

                foundlevel = true;
                break;
            }
        }

        // no more levels??
        // You beat the game!!!!
        // todo: hook up ending sequence?
        if (!foundlevel)
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }

    public static int GetCurrentLevelNum()
    {
        int currentLevelNum = 0;

        if (currentLevel)
        {
            return currentLevel.levelNumber;
        }
        else
        {
#if UNITY_EDITOR
            // figure out our current level
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene != null)
            {
                if (int.TryParse(activeScene.name, out currentLevelNum))
                {
                    return currentLevelNum;
                }
            }
#endif
        }

        return currentLevelNum;
    }

    public static int CalculateStars()
    {
        // 
        // beating it gets you atleast 1
        // more than 3 deaths = 1
        // 3 or less deaths = 2
        // first try is 3 stars = 3
        if (GameManager.playerShip)
        {
            if(GameManager.playerShip.numTimesDied > 3)
            {
                return 1;
            }
            if(GameManager.playerShip.numTimesDied > 0)
            {
                return 2;
            }
            if(GameManager.playerShip.numTimesDied < 1)
            {
                return 3;
            }
        }
        return 3;
    }

    public static void FinishLevel()
    {
        
        int currentLevelNum = GetCurrentLevelNum();

        SaveSystem.Load();

        SaveSystem.SetNumStars(currentLevelNum, CalculateStars());

        SaveSystem.Save();

        // open up our cool "you win the level" sequence
        GameObject winPrefab = Resources.Load<GameObject>("PR_WinPopup");
        GameObject goInstance = GameObject.Instantiate<GameObject>(winPrefab, Vector3.zero, Quaternion.identity);

    }
}