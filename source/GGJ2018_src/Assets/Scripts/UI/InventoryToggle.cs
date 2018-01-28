using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour {
	private Button button = null;
	private bool isInventoryShowing = false;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener( ToggleItemButtons );
	}

	private void Start()
	{
		SetItemButtonsActive( isInventoryShowing );
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener( ToggleItemButtons );
	}

	private void ToggleItemButtons()
	{
		isInventoryShowing = !isInventoryShowing;

		SetItemButtonsActive( isInventoryShowing );
	}

	private void SetItemButtonsActive(bool shouldBeActive)
	{
		HUD hud = GameManager.Instance.GUIRoot.GetComponent<HUD>();
		//hud.fuelPackButton.SetActive( isInventoryShowing );
		hud.attractButton.SetActive( isInventoryShowing );
		//hud.repelButton.SetActive( isInventoryShowing );
		hud.delayFieldButton.SetActive( isInventoryShowing );
		hud.shieldButton.SetActive( isInventoryShowing );
	}
}
