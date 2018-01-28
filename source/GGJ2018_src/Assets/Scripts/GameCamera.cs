using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float bottomMostPos = 39.4f;
    public float upperLimitPos = 100f;
    public Vector3 offsetFromPlayer = Vector3.zero;
    public Vector3 flyingOffsetFromPlayer = Vector3.zero;
    public float maxVelocityOffset = 10f;
    public float velocityOffsetMultiplier = 10f;

    public float followLerpStrength = 0.25f;

    CameraShake shake;

    public static GameCamera Current;

    public float touchMoveSpeed = 3f;

    public float lastResetShipTime = 0f;
    public bool resettingCam = true;

    // Use this for initialization
    void Start()
    {
        shake = GetComponentInChildren<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        bool isFlying = false;

        // follow player ship
        bool shouldFollow = false;
        if (GameManager.playerShip)
        {
            isFlying = GameManager.playerShip.canGo;

            shouldFollow = GameManager.playerShip && GameManager.playerShip.canGo;
            if(resettingCam && GameManager.playerShip.canGo)
            {
                resettingCam = false;
            }
        }
        
        if(!isFlying && Time.time - lastResetShipTime < 2f)
        {
            shouldFollow = true;
            resettingCam = true;
        }

        if(resettingCam)
        {
            shouldFollow = true;
        }

        if (GameManager.playerShip && shouldFollow)
        {
            Vector3 playerPosPlusVelocity = GameManager.playerShip.transform.position;
            if (GameManager.playerShip.body && GameManager.playerShip.body.velocity.magnitude > 0f)
            {
                Vector3 velocityMod = GameManager.playerShip.body.velocity * velocityOffsetMultiplier;
                velocityMod = Vector3.ClampMagnitude(velocityMod, maxVelocityOffset);
                playerPosPlusVelocity += velocityMod;
            }

            Debug.DrawLine(playerPosPlusVelocity, playerPosPlusVelocity + new Vector3(0f, 0f, 1f), Color.yellow);
            Debug.DrawLine(transform.position, transform.position + new Vector3(0f, 0f, 10f), Color.magenta);

            newPos = Vector3.Lerp(transform.position, 
                playerPosPlusVelocity +
                (resettingCam ? offsetFromPlayer : flyingOffsetFromPlayer)
                , followLerpStrength * Time.deltaTime * 
                (resettingCam ? 5f : 1f)
                );
        }
        else
        {
            bool canMoveCam = GameManager.Instance.ItemGrabManager.HeldItem == null;

            if (canMoveCam)
            {
                // While in building mode we can move our camera up+down
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    // Get movement of the finger since last frame
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                    // Move object across XY plane
                    newPos += new Vector3(0f, touchDeltaPosition.y * touchMoveSpeed, 0f);

                }

                // clicking and dragging your mouse moves us up/down
                if (Input.GetMouseButton(0))
                {
                    //Debug.Log("Mouse y: " + Input.GetAxis("Mouse Y").ToString());
                    //Debug.Log("newpos before:" + newPos.ToString());
                    //newPos = newPos + new Vector3(0f, Input.GetAxis("Mouse Y") * 20f, 0f);
                    newPos.y = newPos.y + Input.GetAxis("Mouse Y") * .1f;
                    //Debug.Log("newpos after:" + newPos);
                }
            }
            ///if(Input.mouse)
        }

        // enforce bottom limit
        if (!isFlying && newPos.y < bottomMostPos)
        {
            newPos.y = bottomMostPos;
        }
        // enforce upper limit
        if(newPos.y > upperLimitPos)
        {
            newPos.y = upperLimitPos;
        }

        // enforce sides limits
        if(!isFlying)
        {
            newPos.x = 0f;
        }
        

        transform.position = newPos;

        if(resettingCam)
        {
            if (GameManager.playerShip && !GameManager.playerShip.canGo)
            {
                Vector3 desiredPos = GameManager.playerShip.transform.position;
                desiredPos += offsetFromPlayer;
                if (Vector3.Distance(transform.position, desiredPos) < 0.05f)
                {
                    resettingCam = false;
                }
            }
        }
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
