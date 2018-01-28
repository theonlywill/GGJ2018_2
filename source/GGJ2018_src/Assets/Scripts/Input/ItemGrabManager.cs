using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabManager : MonoBehaviour
{
	private GameObject heldItem = null;

	public GameObject HeldItem
	{
		get { return heldItem; }
	}

	#region Unity Messages
	void Update()
	{
		if( heldItem != null )
		{
			if( Input.GetMouseButton( 0 ) )
			{
				UpdateHeldItemPosition();
			}
			else
			{
				ReleaseItem();
			}
		}
	}
	#endregion Unity Messages

	#region Public Interface
	public void GrabItem( GameObject item )
	{
		heldItem = item;

		Rigidbody2D body = heldItem.GetComponent<Rigidbody2D>();
		if(body != null)
		{
			body.isKinematic = true;
		}
		
		UpdateHeldItemPosition();
	}
	#endregion Public Interface

	#region Helpers
	private void ReleaseItem()
	{
		Rigidbody2D body = heldItem.GetComponent<Rigidbody2D>();
		if( body != null )
		{
			body.isKinematic = false;
		}

		heldItem = null;
	}

	private void UpdateHeldItemPosition()
	{
		Vector3 newPos = WorldPositionFromMousePosition();
		heldItem.transform.position = new Vector3( newPos.x, newPos.y, heldItem.transform.position.z );
	}

	private Vector3 WorldPositionFromMousePosition()
	{
		Vector3 mouse = Input.mousePosition;
		mouse.z = heldItem.transform.position.z;
		Vector3 newPos = Camera.main.ScreenToWorldPoint( mouse );

		return newPos;
	}
	#endregion Helpers
}
