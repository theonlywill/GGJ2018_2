using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : Item
{
    public float amount = 25f;
    bool pickedUp = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(pickedUp)
        {
            return;
        }

        if (collision.attachedRigidbody)
        {
            PlayerShip player = collision.attachedRigidbody.GetComponent<PlayerShip>();
            if (player)
            {
                player.AddFuel(amount);
                pickedUp = true;
                StartCoroutine(SelfDestructRoutine());
            }
        }
    }

    IEnumerator SelfDestructRoutine()
    {
        //Debug.Log("Self destructing");
        yield return null;
        // disable our collider
        //TODO: spawn a pickup fx
        // TODO: Sounds

        Destroy(gameObject);
    }
}
