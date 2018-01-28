using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	#region Singleton Access
	private static GameManager activeInstance = null;
	public static GameManager Instance
	{
		get
		{
			if(activeInstance == null)
			{
				activeInstance = new GameManager();
			}

			return activeInstance;
		}
	}
	#endregion Singleton Access

	private ItemGrabManager itemGrabManager = null;
	public ItemGrabManager ItemGrabManager
	{
		get { return itemGrabManager; }
		set { itemGrabManager = value; }
	}

	private GameObject guiRoot = null;
	public GameObject GUIRoot
	{
		get { return guiRoot; }
		set { guiRoot = value; }
	}

    public static PlayerShip playerShip;

	public HUD HUD
	{
		get { return guiRoot.GetComponent<HUD>(); }
	}

	private GameManager()
	{
		activeInstance = this;
	}
}
