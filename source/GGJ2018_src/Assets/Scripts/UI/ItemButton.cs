using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( Button ) )]
public class ItemButton : MonoBehaviour
{
	public InventoryItem item = null;
	private Button button = null;

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	public void AddItem()
	{
		button.interactable = true;
	}

	public void TakeItem()
	{
		if( !button.interactable )
		{
			return;
		}

		GameObject itemObject = Instantiate<GameObject>(item.itemPrefab);
		itemObject.GetComponent<Item>().WasPlacedByPlayer = true;
		GameManager.Instance.ItemGrabManager.GrabItem( itemObject );

		item.count--;
		if( item.count == 0 )
		{
			button.interactable = false;
		}
	}
}
