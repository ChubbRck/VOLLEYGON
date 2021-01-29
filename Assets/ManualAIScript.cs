﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAIScript : MonoBehaviour
{
    Rigidbody2D rBody;
    public GameObject playerBeingControlled;
    public GameObject ballPrefab;
    public GameObject ball;
    public Transform Target;
    private GameObject mpm;
    private PlayerController pc;
    private float nextSwitchTime = 2f;
    private float directionFactor = 1f;
    public bool isTeammate = false;
    private float ballSpeedToTriggerJump = 7f;
    float maxBallSpeedToTriggerJump = 20f;
    public bool allowGravityChanges = true;
    public bool allowJumps = true;
    public float distanceTolerance = 1.15f;
    public float randomXSpot;
    public float randomXRate;
    private int whichTeam;

    public void Start()
    {
        pc = playerBeingControlled.GetComponent<PlayerController>();
        whichTeam = playerBeingControlled.GetComponent<PlayerController>().team;
        rBody = playerBeingControlled.GetComponent<Rigidbody2D>();
        mpm = GameObject.FindWithTag("MidpointMarker");
        nextSwitchTime = Random.Range(.4f, .8f);

        if (whichTeam == 2)
        {
            randomXSpot = Random.Range(3f, 16f);
        } else if (whichTeam == 1){
            randomXSpot = Random.Range(-3f, -16f);
        }
        randomXRate = Random.Range(.1f, .5f);
    }

    public void FixedUpdate()
    {

        transform.position = playerBeingControlled.transform.position;

    }

    public void OriginalDirection()
    {
        directionFactor = 1f;
    }

    public void GoBackToNeutral()
    {
        if (rBody.position.x > randomXSpot + .2f)
        {
            pc.virtualButtons.horizontal = -randomXRate;

        }

        else if (rBody.position.x < randomXSpot - .2f)
        {
            pc.virtualButtons.horizontal = randomXRate;
        } else
        {
            if (whichTeam == 2)
            {
                randomXSpot = Random.Range(3f, 16f);
            }
            else if (whichTeam == 1)
            {
                randomXSpot = Random.Range(-3f, -16f);
            }
        }
    }
    public void Update()
    {
        if (!pc.isJumping)
        {
            directionFactor = 1f;
        }
        if (Target == null)
        {
            GoBackToNeutral();
            if (GameObject.FindWithTag("Ball"))
            {
                Target = GameObject.FindWithTag("Ball").transform;
            }
        }
        else
        {
            float distanceToBall = Mathf.Abs(Target.transform.position.x - rBody.position.x);
            if (distanceToBall > distanceTolerance)
            {
                // Move toward the ball.
                if (Target.transform.position.x < rBody.position.x)
                {
                    float amountToMove = 1f;
                    if (Mathf.Abs(Target.transform.position.x - rBody.position.x) < 2f)
                    {
                        amountToMove = (Target.transform.position.x - rBody.position.x) / 2f;
                        amountToMove = .8f;
                    }
                    if (Mathf.Abs(Target.transform.position.x - rBody.position.x) < 1f)
                    {
                        amountToMove = (Target.transform.position.x - rBody.position.x) / 2f;
                        amountToMove = .5f;
                    }
                    if (Mathf.Abs(Target.transform.position.x - rBody.position.x) < .2f)
                    {
                        amountToMove = (Target.transform.position.x - rBody.position.x) / 2f;
                        amountToMove = .5f;
                    }
                    playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = -amountToMove; //TODO: Change this to be proportionate to the distance
                }
                else if (Target.transform.position.x > rBody.position.x)
                {
                    float amountToMove = 1f;
                    if (Mathf.Abs(Target.transform.position.x - rBody.position.x) < 2f)
                    {
                        amountToMove = (Target.transform.position.x - rBody.position.x) / 2f;
                        amountToMove = .8f;
                    }
                    if (Mathf.Abs(Target.transform.position.x - rBody.position.x) < 1f)
                    {
                        amountToMove = (Target.transform.position.x - rBody.position.x) / 2f;
                        amountToMove = .5f;
                    }
                    if (Mathf.Abs(Target.transform.position.x - rBody.position.x) < .2f)
                    {
                        amountToMove = (Target.transform.position.x - rBody.position.x) / 2f;
                        amountToMove = .5f;
                    }
                    playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = amountToMove * directionFactor;
                }

                if (Target.transform.position.x > .5f && pc.team == 1) // TODO: sign based on which team.
                {
                    //behavior to move to center here.
                    GoBackToNeutral();
                    //playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = 0f;
                }

                if (Target.transform.position.x < -.5f && pc.team == 2) // TODO: sign based on which team.
                {
                    //behavior to move to center here.
                    //playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = 0f;
                    GoBackToNeutral();
                }

                if (Target.GetComponent<Rigidbody2D>().isKinematic)
                {
                    //behavior to move to center here.
                    GoBackToNeutral();
                    //playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = 0f;
                }
            }
            else
            {
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal *= .95f;
            }

            if (allowGravityChanges)
            {
                if (Mathf.Sign(Target.GetComponent<Rigidbody2D>().gravityScale) != Mathf.Sign(rBody.gravityScale) || Mathf.Sign(Target.GetComponent<Rigidbody2D>().gravityScale) == Mathf.Sign(rBody.gravityScale) && Target.GetComponent<BallScript>().timer < nextSwitchTime)
                {
                    playerBeingControlled.GetComponent<PlayerController>().virtualButtons.grav = true;
                    directionFactor = -3f;
                    Invoke("OriginalDirection", .5f);
                    nextSwitchTime = Random.Range(.4f, .8f);
                }
                else
                {
                    playerBeingControlled.GetComponent<PlayerController>().virtualButtons.grav = false;
                }
            }
            //if (isTeammate)
            //{
            //    if (Mathf.Sign(Target.GetComponent<Rigidbody2D>().gravityScale) == Mathf.Sign(rBody.gravityScale) || Mathf.Sign(Target.GetComponent<Rigidbody2D>().gravityScale) != Mathf.Sign(rBody.gravityScale) && Target.GetComponent<BallScript>().timer < nextSwitchTime)
            //    {
            //        playerBeingControlled.GetComponent<PlayerController>().virtualButtons.grav = true;
            //        directionFactor = -3f;
            //        Invoke("OriginalDirection", .5f);
            //        nextSwitchTime = Random.Range(.4f, .8f);
            //    }
            //    else
            //    {
            //        playerBeingControlled.GetComponent<PlayerController>().virtualButtons.grav = false;
            //    }
            //}

            //Flip this so the trigger is always facing the right way.
            float factor = 1f;
            if (Mathf.Sign(Target.GetComponent<Rigidbody2D>().gravityScale) != Mathf.Sign(rBody.gravityScale)){
                factor = .4f;
            }
            transform.localScale = new Vector3(1f, rBody.gravityScale*factor, 1f);
            


        }


    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball" && allowJumps)
        {
            float ballSpeed = other.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            float ballYSpeed = other.gameObject.GetComponent<Rigidbody2D>().velocity.y;
            Debug.Log("Ball moving at");
            Debug.Log(Mathf.Abs(ballSpeed));
            //TODO: Need a way to deal with this for both gravities
            float distanceToBall = Mathf.Abs(Target.transform.position.x - rBody.position.x);
            if (distanceToBall < .65f && ballSpeed < 3f && ballYSpeed < 2.5f)
            {
                Debug.Log("ball seems to be stuck, let's get it out");
                // choose a direction to move toward the Net
                float whichWay = 3f;
                if (pc.team == 2) { whichWay = -3f; }
                if (!pc.isJumping)
                {
                    playerBeingControlled.GetComponent<PlayerController>().virtualButtons.horizontal = whichWay;
                }
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.jump = true;
            } // TODO: Change this number
            else if (Mathf.Abs(other.gameObject.GetComponent<Rigidbody2D>().velocity.x) > ballSpeedToTriggerJump && Mathf.Abs(other.gameObject.GetComponent<Rigidbody2D>().velocity.x) < maxBallSpeedToTriggerJump)
            {
                playerBeingControlled.GetComponent<PlayerController>().virtualButtons.jump = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            playerBeingControlled.GetComponent<PlayerController>().virtualButtons.jump = false;
        }
    }
}
