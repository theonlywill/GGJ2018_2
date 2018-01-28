using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public bool WasPlacedByPlayer = false;

	public void SetCollidersEnabled(bool shouldBeEnabled)
	{
		Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
		foreach(Collider2D coll in colliders)
		{
			coll.enabled = shouldBeEnabled;
		}
	}
}
