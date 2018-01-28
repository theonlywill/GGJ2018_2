using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipLauncher : MonoBehaviour
{
	private Button button = null;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener( Launch );

		GameManager.Instance.ShipLauncher = this;
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener( Launch );
	}

	private void Launch()
	{
		GameManager.playerShip.LaunchShip();
		button.gameObject.SetActive( false );
	}

	public void ResetLauncher()
	{
		button.gameObject.SetActive( true );
	}

	//private void Update()
	//{
	//	if( Input.GetMouseButtonDown( 0 ) )
	//	{
	//		Debug.Log( "I'm trying I swear!" );
	//		Vector3 origin = new Vector3( Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane );
	//		Ray ray = Camera.main.ScreenPointToRay( origin );
	//		Debug.DrawRay( ray.origin, ray.direction * 80f, Color.green, 10f );

	//		RaycastHit2D[] hitInfo = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
	//		//if( hitInfo )
	//		//{
	//		//	Debug.Log( "I hit something! " + hitInfo.collider.name );
	//		//	PlayerShip player = hitInfo.transform.gameObject.GetComponent<PlayerShip>();
	//		//	if( player != null )
	//		//	{
	//		//		Debug.Log( "Extreme woot!" );
	//		//		player.LaunchShip();
	//		//		enabled = false;
	//		//	}
	//		//}
	//		foreach(RaycastHit2D hit in hitInfo)
	//		{
	//			Debug.Log( "Hit " + hit.collider.name );
	//			PlayerShip player = hit.transform.gameObject.GetComponent<PlayerShip>();
	//			if(player != null)
	//			{
	//				Debug.Log( "Extreme woot!" );
	//				player.LaunchShip();
	//				enabled = false;
	//				break;
	//			}
	//		}
	//	}
	//}

	//private void OnMouseDown()
	//{
	//	if( enabled )
	//	{
	//		playerShip.LaunchShip();
	//		enabled = false;
	//	}
	//}
}
