using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float bottomMostPos = 39.4f;
    public Vector3 offsetFromPlayer = Vector3.zero;
    public float maxVelocityOffset = 10f;
    public float velocityOffsetMultiplier = 10f;

    public float followLerpStrength = 0.25f;

    CameraShake shake;

    public static GameCamera Current;

    public float touchMoveSpeed = 3f;

    // Use this for initialization
    void Start()
    {
        shake = GetComponentInChildren<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        // follow player ship
        if (GameManager.playerShip && GameManager.playerShip.canGo)
        {
            Vector3 playerPosPlusVelocity = GameManager.playerShip.transform.position;
            if (GameManager.playerShip.body && GameManager.playerShip.body.velocity.magnitude > 0f)
            {
                Vector3 velocityMod = GameManager.playerShip.body.velocity * velocityOffsetMultiplier;
                velocityMod = Vector3.ClampMagnitude(velocityMod, maxVelocityOffset);
                playerPosPlusVelocity += velocityMod;
            }
            newPos = Vector3.Lerp(newPos, playerPosPlusVelocity + offsetFromPlayer, followLerpStrength * Time.deltaTime);
        }
        else
        {
            // While in building mode we can move our camera up+down
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Get movement of the finger since last frame
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                // Move object across XY plane
                newPos += new Vector3(0f, touchDeltaPosition.y * touchMoveSpeed, 0f);
                
            }

            // clicking and dragging your mouse moves us up/down
            if(Input.GetMouseButton(0))
            {
                //Debug.Log("Mouse y: " + Input.GetAxis("Mouse Y").ToString());
                //Debug.Log("newpos before:" + newPos.ToString());
                //newPos = newPos + new Vector3(0f, Input.GetAxis("Mouse Y") * 20f, 0f);
                newPos.y = newPos.y + Input.GetAxis("Mouse Y") * 1f;
                //Debug.Log("newpos after:" + newPos);
            }

            ///if(Input.mouse)
        }

        // enforce bottom limit
        if (newPos.y < bottomMostPos)
        {
            newPos.y = bottomMostPos;
        }
        // enforce sides limits
        newPos.x = 0f;

        transform.position = newPos;
    }

    [ContextMenu("Test Shake")]
    public void TestShake()
    {
        Shake(1f, 2f);
    }

    public void Shake(float amount, float duration)
    {
        if (shake)
        {
            shake.shake = duration;
            shake.shakeAmount = amount;
        }
    }

    public void OnEnable()
    {
        Current = this;
    }
}
