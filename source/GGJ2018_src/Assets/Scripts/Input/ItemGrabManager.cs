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
		else if( Input.GetMouseButtonDown( 0 ) )
		{
			Vector3 origin = new Vector3( Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane );
			Ray ray = Camera.main.ScreenPointToRay( origin );

			RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
			if( hitInfo )
			{
				Item item = hitInfo.transform.gameObject.GetComponent<Item>();
				if( item != null && item.WasPlacedByPlayer )
				{
					GrabItem( item.gameObject );
				}
			}
		}
	}
	#endregion Unity Messages

	#region Public Interface
	public void GrabItem( GameObject item )
	{
		heldItem = item;

		Rigidbody2D body = heldItem.GetComponent<Rigidbody2D>();
		if( body != null )
		{
			body.isKinematic = true;
		}

		Item itemComp = item.GetComponent<Item>();
		if( itemComp != null )
		{
			itemComp.SetCollidersEnabled( false );
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

		Item itemComp = heldItem.GetComponent<Item>();
		if( itemComp != null )
		{
			itemComp.SetCollidersEnabled( true );
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
		//Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(mouse), )
		Plane gameplane = new Plane(Vector3.forward,0f);
		float dist = 0f;
		gameplane.Raycast( Camera.main.ScreenPointToRay( mouse ), out dist );
		mouse.z = dist;
		Vector3 newPos = Camera.main.ScreenToWorldPoint( mouse );

		return newPos;
	}
	#endregion Helpers
}
