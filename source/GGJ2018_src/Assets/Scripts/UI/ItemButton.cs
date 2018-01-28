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
		button.onClick.AddListener( TakeItem );
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener( TakeItem );
	}

	public void TakeItem()
	{
		GameObject itemObject = Instantiate<GameObject>(item.itemPrefab);
		GameManager.Instance.ItemGrabManager.GrabItem( itemObject );

		item.count--;
		if( item.count == 0 )
		{
			gameObject.SetActive( false );
		}
	}
}
