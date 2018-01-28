using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shake = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
	
	void OnPreCull()
	{
		if (shake > 0)
		{
            float fadeMultiplier = 1f - shake;
            fadeMultiplier = 1f - Mathf.Clamp(fadeMultiplier, 0f, 1f);

			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * fadeMultiplier;
			
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}