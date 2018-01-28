using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditWindow : EditorWindow
{
    Vector2 scroll = Vector2.zero;

    /// <summary>
    /// Window drawing operations
    /// </summary>
    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        List<LevelInfo> levels = new List<LevelInfo>();
        levels.AddRange(Resources.LoadAll<LevelInfo>("Levels"));

        levels.Sort(SortByName);

        for(int i = 0; i < levels.Count; i++)
        {
            LevelInfo level = levels[i];
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.ObjectField(level);
            level.levelNumber = EditorGUILayout.IntField(level.name +":", level.levelNumber);
            level.numAttract = EditorGUILayout.IntField("Attract:", level.numAttract);
            level.numFuelPacks = EditorGUILayout.IntField("Fuel:", level.numAttract);
            level.numRepel = EditorGUILayout.IntField("Repel:", level.numAttract);
            level.numDelay = EditorGUILayout.IntField("Delay:", level.numAttract);
            level.numShield = EditorGUILayout.IntField("Shield:", level.numAttract);
            level.numMissile = EditorGUILayout.IntField("Missile:", level.numAttract);
            EditorGUILayout.EndHorizontal();

        }

        

        EditorGUILayout.EndScrollView();
    }
    
    private static int SortByName(LevelInfo a, LevelInfo b)
    {
        return a.levelNumber.CompareTo(b.levelNumber);
    }

    /// <summary>
    /// Retrives the TransformUtilities window or creates a new one
    /// </summary>
    [MenuItem("Tools/Level Manager")]
    static void Init()
    {
        LevelEditWindow window = (LevelEditWindow)EditorWindow.GetWindow(typeof(LevelEditWindow));
        window.Show();
    }
}
