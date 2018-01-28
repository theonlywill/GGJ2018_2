using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour {
	public PlayerShip player = null;

	private Slider slider = null;

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	private void Update()
	{
		slider.value = player.fuel / player.maxFuel;
	}
}
