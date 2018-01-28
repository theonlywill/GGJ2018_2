using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public GameObject fuelPackButton = null;
	public GameObject attractButton = null;
	public GameObject repelButton = null;
	public GameObject delayFieldButton = null;
	public GameObject shieldButton = null;
	public GameObject missileButton = null;

	public Button toggleInvBtn = null;

	// Default to true so we can just call Toggle on Start.
	private bool isInventoryShowing = true;

	private int numItemTypesWithItemsRemaining = 0;

	private void Awake()
	{
		GameManager.Instance.ItemGrabManager = gameObject.AddComponent<ItemGrabManager>();
		GameManager.Instance.GUIRoot = gameObject;
	}

	private void Start()
	{
		ToggleItemButtons();
	}

	public void AssignAttractItem( InventoryItem item )
	{
		AssignItemToButton( item, attractButton );
	}

	public void AssignDelayItem( InventoryItem item )
	{
		AssignItemToButton( item, delayFieldButton );
	}

	public void AssignFuelItem( InventoryItem item )
	{
		AssignItemToButton( item, fuelPackButton );
	}

	public void AssignMissileItem( InventoryItem item )
	{
		AssignItemToButton( item, missileButton );
	}

	public void AssignRepelItem( InventoryItem item )
	{
		AssignItemToButton( item, repelButton );
	}

	public void AssignShieldItem( InventoryItem item )
	{
		AssignItemToButton( item, shieldButton );
	}

	public void FinishAssigningButtons()
	{
		toggleInvBtn.interactable = ( numItemTypesWithItemsRemaining > 0 );
	}

	public void ToggleItemButtons()
	{
		isInventoryShowing = !isInventoryShowing;

		SetButtonActive( isInventoryShowing, attractButton );
		SetButtonActive( isInventoryShowing, delayFieldButton );
		SetButtonActive( isInventoryShowing, shieldButton );
		SetButtonActive( isInventoryShowing, fuelPackButton );
		SetButtonActive( isInventoryShowing, repelButton );

		if(!isInventoryShowing)
		{
			numItemTypesWithItemsRemaining = 0;

			CheckItemTypeForItemsLeft( attractButton );
			CheckItemTypeForItemsLeft( delayFieldButton );
			CheckItemTypeForItemsLeft( shieldButton );
			CheckItemTypeForItemsLeft( fuelPackButton );
			CheckItemTypeForItemsLeft( repelButton );

			FinishAssigningButtons();
		}
	}

	private void CheckItemTypeForItemsLeft(GameObject button)
	{
		if(button != null)
		{
			ItemButton btn = button.GetComponent<ItemButton>();
			if(btn.item.count > 0)
			{
				numItemTypesWithItemsRemaining++;
			}
		}
	}

	private void SetButtonActive( bool wantsToShow, GameObject button )
	{
		if( button != null )
		{
			ItemButton btn = button.GetComponent<ItemButton>();
			button.SetActive( wantsToShow && btn.item.count > 0 );
		}
	}

	private void AssignItemToButton( InventoryItem item, GameObject button )
	{
		if( button != null )
		{
			ItemButton btn = button.GetComponent<ItemButton>();
			btn.item = item;

			if( item.count == 0 )
			{
				button.SetActive( false );
			}
			else
			{
				button.SetActive( true );
				numItemTypesWithItemsRemaining++;
			}
		}
	}
}
