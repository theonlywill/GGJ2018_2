using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerShip))]
public class PlayerShipEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerShip ship = (PlayerShip)target;

        float speed = 0f;
        if(ship.body)
        {
            speed = ship.body.velocity.magnitude;
        }
        EditorGUILayout.LabelField("Current Speed: " + speed);

        if(GUILayout.Button("LAUNCH!!!"))
        {
            
            ship.LaunchShip();
        }
    }
}
