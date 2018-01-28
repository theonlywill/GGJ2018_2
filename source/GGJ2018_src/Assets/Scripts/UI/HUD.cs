using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
	public GameObject fuelPackButton = null;
	public GameObject attractButton = null;
	public GameObject repelButton = null;
	public GameObject delayFieldButton = null;
	public GameObject shieldButton = null;
	public GameObject missileButton = null;

	private void Awake()
	{
		GameManager.Instance.ItemGrabManager = gameObject.AddComponent<ItemGrabManager>();
		GameManager.Instance.GUIRoot = gameObject;
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
