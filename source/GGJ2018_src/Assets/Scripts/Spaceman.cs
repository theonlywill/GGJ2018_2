using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceman : MonoBehaviour {

    public float forceMin = 0f;
    public float forceMax = 0f;


	// Use this for initialization
	void Start () {

        // add a tiiiny amount of force to him, so he'll float
        Rigidbody2D spacemanBody = GetComponent<Rigidbody2D>();
        if (spacemanBody)
        {
            float forcePowerX = Random.Range(forceMin, forceMax);
            float forcePowerY = Random.Range(forceMin, forceMax);
            spacemanBody.AddForce(new Vector2(Random.Range(-forcePowerX, forcePowerX), Random.Range(-forcePowerY, forcePowerY)), ForceMode2D.Impulse);
        }

        // random rotation
        transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
        //transform.rotation
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
