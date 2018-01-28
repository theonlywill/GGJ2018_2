using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShip : MonoBehaviour
{
    [Header("Fuel")]
    [Range(0f, 100f)]
    public float fuel = 100f;
    public float maxFuel = 100f;
    public float forwardSpeed = 3f;

    public float fuelConsumptionRate = 5f;

    public Rigidbody2D body;

    public UnityEvent onFuelEmpty;

    public bool canGo = false;

    [Header("Death")]
    public GameObject explosionPrefab;
    public GameObject deadSpacemanPrefab;
    public float deathShakePower = 0.05f;
    public float deathShakeDuration = 2f;

    public GameObject model;

    List<DelayField> delayFieldsImIn = new List<DelayField>();
    List<GravityField> gravityFieldsImIn = new List<GravityField>();
    List<RepulseField> repulseFieldsImIn = new List<RepulseField>();

    List<ParticleSystem> thrusterParticles = new List<ParticleSystem>();
    public GameObject thrusterFX;

    public float rotationSpeedDegreesPerSec = 60f;

    [Header("Launch")]
    public float launchShakePower = 0.005f;
    public float launchShakeDuration = 3f;



    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.simulated = false;

        // find our thrusters (we'll just look for all under thruster fx gameobject)
        if (thrusterFX)
        {
            thrusterParticles.AddRange(thrusterFX.GetComponentsInChildren<ParticleSystem>());
        }

        SetThrusters(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canGo)
        {
            UpdateFuel();

            PointTowardsVelocity();
        }
    }

    [ContextMenu("Reset Ship")]
    public void ResetShip()
    {
        StartCoroutine(ResetShipRoutine());

    }

    IEnumerator ResetShipRoutine()
    {
        
        //model.SetActive(true);
        ResetFuel();
        body.simulated = false;
        SetThrusters(false);
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;

        delayFieldsImIn.Clear();
        gravityFieldsImIn.Clear();
        repulseFieldsImIn.Clear();

        //reset our rotation
        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1f);

        // Place me on the launchpad
        transform.position = Vector3.zero;
        model.SetActive(true);
    }

    [ContextMenu("LAUNCH")]
    public void LaunchShip()
    {
        canGo = true;
        body.simulated = true;
        // todo: play some launch sfx
        SetThrusters(true);

        GameCamera.Current.Shake(launchShakePower, launchShakeDuration);
    }

    public void SetThrusters(bool i_on)
    {
        for (int i = 0; i < thrusterParticles.Count; i++)
        {
            if (thrusterParticles[i])
            {
                ParticleSystem.EmissionModule em = thrusterParticles[i].emission;
                em.enabled = i_on;
                // you don't need to reassign the em module
            }
        }
    }

    public void AddFuel(float amountToAdd)
    {
        float newAmount = fuel + amountToAdd;
        if (newAmount > maxFuel)
        {
            newAmount = maxFuel;
        }

        fuel = newAmount;
    }

    public void ResetFuel()
    {
        fuel = maxFuel;
    }

    void UpdateFuel()
    {
        float newFuel = fuel - fuelConsumptionRate * Time.deltaTime;
        if (newFuel < 0f)
        {
            newFuel = 0f;
        }

        if (newFuel <= 0f && fuel > 0f)
        {
            OnFuelEmpty();
        }

        fuel = newFuel;
    }

    void OnFuelEmpty()
    {
        onFuelEmpty.Invoke();

        SetThrusters(false);
    }

    public void FixedUpdate()
    {
        if (!canGo)
        {
            // make sure our gravity doesn't move us
            //body.AddForce(Physics.gravity * -1f * body.gravityScale);
            return;
        }
        if (fuel > 0f)
        {
            float maxSpeed = CalcMaxSpeed();
            if (body.velocity.magnitude < maxSpeed)
            {
                body.AddForce(transform.up * forwardSpeed * Time.deltaTime, ForceMode2D.Force);
            }
        }

        ApplyDelayFieldBrakes();
        ApplyGravityFields();
        ApplyRepulseFields();

        //PointTowardsVelocity();
    }

    void ApplyDelayFieldBrakes()
    {
        // while in delay fields we'll hit the brakes if we are going too fast
        float maxSpeed = CalcMaxSpeed();
        if (body.velocity.magnitude > maxSpeed)
        {
            // HIT THE BRAKES!!!!
            body.AddForce(body.velocity * -1f * DelayField.BRAKE_POWER);
        }
    }

    void ApplyGravityFields()
    {
        for (int i = 0; i < gravityFieldsImIn.Count; i++)
        {
            if (gravityFieldsImIn[i])
            {
                body.AddForce((gravityFieldsImIn[i].transform.position - transform.position).normalized * gravityFieldsImIn[i].power * Time.fixedDeltaTime);
            }
        }
    }

    void ApplyRepulseFields()
    {
        for (int i = 0; i < repulseFieldsImIn.Count; i++)
        {
            if (repulseFieldsImIn[i])
            {
                body.AddForce((repulseFieldsImIn[i].transform.position - transform.position).normalized * -repulseFieldsImIn[i].power * Time.fixedDeltaTime);
            }
        }
    }

    void PointTowardsVelocity()
    {
        if (body.velocity.magnitude != 0f)
        {
            Quaternion desiredRotation = Quaternion.identity;
            Vector3 destPoint = transform.position + GetWorldVelocity3D();
            desiredRotation = Quaternion.LookRotation(Vector3.forward, (destPoint - transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeedDegreesPerSec);
            //Quaternion.LookRotation()
            //transform.LookAt(transform.position + GetWorldVelocity3D(), Vector3.up);
            //transform.Rotate(Vector3.right, 90f );//bleh, because my forward is actually different.
        }
    }

    Vector3 GetWorldVelocity3D()
    {
        if (body)
        {
            return new Vector3(body.velocity.x, body.velocity.y, 0f);
        }

        return Vector3.zero;
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (body)
        {

            Gizmos.DrawWireSphere(transform.position + GetWorldVelocity3D() * 10f, 10f);
        }
    }
#endif //UNITY_EDITOR

    float CalcMaxSpeed()
    {
        float max = float.PositiveInfinity;
        for (int i = 0; i < delayFieldsImIn.Count; i++)
        {
            if (delayFieldsImIn[i])
            {
                if (max > delayFieldsImIn[i].maxSpeed)
                {
                    max = delayFieldsImIn[i].maxSpeed;
                }
            }
        }
        return max;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        DelayField delayField = collision.GetComponent<DelayField>();
        if (delayField)
        {
            delayFieldsImIn.Add(delayField);
        }

        GravityField gravityField = collision.GetComponent<GravityField>();
        if (gravityField)
        {
            gravityFieldsImIn.Add(gravityField);
        }

        RepulseField repulseField = collision.GetComponent<RepulseField>();
        if (repulseField)
        {
            repulseFieldsImIn.Add(repulseField);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        DelayField delayField = other.GetComponent<DelayField>();
        if (delayField)
        {
            delayFieldsImIn.Remove(delayField);
        }

        GravityField gravityField = other.GetComponent<GravityField>();
        if (gravityField)
        {
            gravityFieldsImIn.Remove(gravityField);
        }

        RepulseField repulseField = other.GetComponent<RepulseField>();
        if (repulseField)
        {
            repulseFieldsImIn.Remove(repulseField);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (canGo)
        {
            WinPortal winPortal = collision.collider.GetComponent<WinPortal>();
            if (winPortal)
            {
                // YOU WIN!!!!
                Debug.Log("You win!!!!");

                canGo = false;

                gameObject.SetActive(false);

                LevelSystem.FinishLevel();

                return;
            }

            // fuel pickups are handled in fuelpickup.cs.ontriggerenter (cause they have trigger volumes)
            

            // create our explosion and our dead space man
            if (explosionPrefab)
            {
                GameObject goExplosion = GameObject.Instantiate<GameObject>(explosionPrefab, transform.position, Quaternion.identity, null);
            }

            if (deadSpacemanPrefab)
            {
                GameObject goSpaceman = GameObject.Instantiate<GameObject>(deadSpacemanPrefab, transform.position, Quaternion.identity, null);

            }

            GameCamera.Current.Shake(deathShakePower,deathShakeDuration);

            SetThrusters(false);

            // hide our model
            model.SetActive(false);
            canGo = false;
            body.simulated = false;

            ResetShip();
        }
    }

    public void OnEnable()
    {
        GameManager.playerShip = this;
    }

    public void OnDisable()
    {

    }

    
}
