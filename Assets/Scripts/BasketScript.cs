﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript : MonoBehaviour
{
    private BoxCollider2D hotZone;
    private GameObject ICM;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        // Identify this basket's hot zone
        hotZone = GetComponent<BoxCollider2D>();

        // Identify the individual challenge manager
        ICM = GameObject.FindWithTag("IndividualChallengeManager");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("Triggered!");
            // tell the ball to destroy itself
            // BroadcastMessage to the individual challenge manager that a basket has been scored
            ICM.BroadcastMessage("OnBasketScored");

            //Fire the particle system! TODO: There must be a better way than this.,.. yikes.
            explosion.GetComponent<ParticleSystem>().Play();

            // Could these two be combined?
            other.gameObject.GetComponent<BallScript>().FireExplosion();
            other.gameObject.GetComponent<BallScript>().DestroyBall();
        }
    }
}
