﻿using UnityEngine;
using System.Collections;
using PigeonCoopToolkit.Effects.Trails;

public class AIControllerScript : MonoBehaviour {

	public float speed = 5f;
	public float spinPower = -150f;
	private float startSpeed;
	private float startMass;
	private float startJumpPower;
	public float jumpPower = 750f;
	public bool isJumping = false;
	public string horiz = "Horizontal_P1";
	public string jumpButton = "Jump_P1";
	public string gravButton = "Grav_P1";
	public int team = 1;
	public float startingGrav = 1;
	public int playerID;
	public float moveHorizontal;
	public Mesh meshTypeOne;
	public Mesh meshTypeTwo;
	public int playerType = 0;
	public PolygonCollider2D trianglePC, trapezoidPC;
	private bool canMove;
	public bool willJump = true;
    public bool willGravChange = true;
	public AudioClip jumpSound1;
	public AudioClip jumpSound2;
	public AudioClip landSound;
	public AudioClip collideWithBallSound1;
	public AudioClip collideWithBallSound2;
	public AudioClip collideWithBallSoundBig;

	public TextMesh pandemoniumCounter;

    // Variables that refine the AIs movement

	public float distanceMaxThreshold;
	public float distanceMinThreshold;
    public float verticalMaxDistance = 20f; // Set arbitrarily high.
    public float maxHorizontalJumpDistance = 2f; // 2f is a legacy value and probably too high.
    public float responsivenessRate;
    public float netSafeDistance = 0f;
    public float backWallSafeDistance = 17.8f;
    public float distanceToTriggerJump = 4f;
    public float changeDirectionRate = .7f;
    public bool moveAwayOnIdle = false;
	public GameObject trail;
	public GameObject ball;

	public Sprite squareSprite;
	public Sprite circleSprite;
	public Sprite triangleSprite;
	public Sprite trapezoidSprite;

	public float penaltyTimer;
	private bool penaltyTimerActive = false;

	private float speedPowerupTimer;
	private bool speedPowerupActive = false;

	private float sizePowerupTimer;
	private bool sizePowerupActive = false;

	private float pandemoniumTimer;
	private bool pandemoniumPowerupActive = false;

	// Different base stats for each playerType

	private float squareJumpPower = 2000f;
	private float squareSpeed = 15f;
	private float squareSpinRate = -150f;

	private float circleJumpPower = 2200f;
	private float circleSpeed = 18f;
	private float circleSpinRate = -100f;

	private float triangleJumpPower = 2000f;
	private float triangleSpeed = 18f;
	private float triangleSpinRate = -200f;

	private float trapezoidJumpPower = 1800f;
	private float trapezoidSpeed = 12f;
	private float trapezoidSpinRate = -150f;


	Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        ball = GameObject.FindWithTag("Ball");
        moveHorizontal = 0f;
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = startingGrav;
		startMass = rb.mass;
		canMove = true;
		pandemoniumCounter.GetComponent<TextMesh> ().color = new Vector4(0f, 0f, 0f, 0f);


		if (playerType == 0) {
			gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			gameObject.GetComponent<SpriteRenderer> ().sprite = squareSprite;
			jumpPower = squareJumpPower;
			speed = squareSpeed;
			spinPower = squareSpinRate;
		}

		if (playerType == 1) {
			gameObject.GetComponent<CircleCollider2D> ().enabled = true;
			gameObject.GetComponent<SpriteRenderer> ().sprite = circleSprite;
			jumpPower = circleJumpPower;
			speed = circleSpeed;
			spinPower = circleSpinRate;
		}

		if (playerType == 2) {
			trianglePC.enabled = true;
			gameObject.GetComponent<SpriteRenderer> ().sprite = triangleSprite;
			jumpPower = triangleJumpPower;
			speed = triangleSpeed;
			spinPower = triangleSpinRate;
		}

		if (playerType == 3) {
			trapezoidPC.enabled = true;
			gameObject.GetComponent<SpriteRenderer> ().sprite = trapezoidSprite;
			jumpPower = trapezoidJumpPower;
			speed = trapezoidSpeed;
			spinPower = trapezoidSpinRate;
		}

		startJumpPower = jumpPower;
		startSpeed = speed;

	}

	void Update(){
//        Debug.Log(moveHorizontal);

        if (ball == null)
        {
            CheckForBall();
        }

        CheckForMove ();
		
        if (willJump) {
			CheckForJump ();
		}

        if (willGravChange)
        {
            CheckForGravChange();
        }

		ClampPosition ();

		ManagePowerups ();
	}

    void CheckForBall()
    {
        ball = GameObject.FindWithTag("Ball");
    }

    bool IsOnSameSide(GameObject currentBall)
    {
        if ((transform.position.x > 0 && currentBall.transform.position.x > 0) || (transform.position.x > 0 && currentBall.transform.position.x > 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckForMove(){
        if (ball != null)
        {
           
            // roll a 'responsiveness' value. Only move each frame if this value is greater than the public 'responsivenessRate' property.
            float chanceToRespond = Random.Range(0f, 100.0f);

            if (chanceToRespond >= responsivenessRate)
            {


                // only move if the ball is close, but not too close
                if (Mathf.Abs(ball.transform.position.x - transform.position.x) <= distanceMaxThreshold && Mathf.Abs(ball.transform.position.y - transform.position.y) <= verticalMaxDistance && Mathf.Abs(ball.transform.position.x - transform.position.x) >= distanceMinThreshold && IsOnSameSide(ball))
                {
                    // Move toward the ball... but not if too close to the net as defined by netSafeDistance (default 0)
                    if (ball.transform.position.x < transform.position.x)
                    {
                        moveHorizontal = -1f;
                        //if (Mathf.Abs(transform.position.x) <= netSafeDistance && team == 2 || Mathf.Abs(transform.position.x) >= backWallSafeDistance && team == 2)
                        //{

                        //    moveHorizontal = 0f;
                        //}
                    }
                    else if (ball.transform.position.x > transform.position.x)
                    {
                        moveHorizontal = 1f;
                        //if (Mathf.Abs(transform.position.x) <= netSafeDistance && team == 1 || Mathf.Abs(transform.position.x) >= backWallSafeDistance && team == 1)
                        //{
                        //    moveHorizontal = 0f;
                        //}
                    }
                }
                else
                {
                    // Not sure the best way to do this... I want to have an alternate behavior here. Should it just be a flag (as is)? Or should it be another component to handle this behavior? 
                    if (moveAwayOnIdle)
                    {
                        // Determine which side this AI is on and move toward the back wall
                        switch (team) {
                            // NOTE: 10f is the 'mid point' of each side
                            case 1:
                                if (transform.position.x > -10f)
                                {
                                    moveHorizontal = -.2f;
                                } 
                                else if (transform.position.x < -10f)
                                {
                                    moveHorizontal = .2f;
                                }
                                else
                                {
                                    moveHorizontal = 0;
                                }
                                break;

                            case 2:
                                if (transform.position.x < 10f)
                                {
                                    moveHorizontal = .2f;
                                } 
                                else if (transform.position.x > 10f)
                                {
                                    moveHorizontal = -.2f;
                                }
                                else
                                {
                                    moveHorizontal = 0;
                                }
                                break;
                        }
                    }
                    else
                    {
                        moveHorizontal = 0;
                    }
                }

            }

           


            if (isJumping)
            {
                GetComponent<Rigidbody2D>().angularVelocity = (moveHorizontal * spinPower * rb.gravityScale);
            }


            Vector3 v3 = GetComponent<Rigidbody2D>().velocity;
            v3.x = Mathf.Lerp(GetComponent<Rigidbody2D>().velocity.x, (moveHorizontal * speed), changeDirectionRate);

            GetComponent<Rigidbody2D>().velocity = v3;
        }
	}

	void CheckForJump(){

        // Only jump if ball is above (or below, depending on grav state) shape and within a certain distance (distanceToTriggerJump, set to default 4f)
        // TODO: There should be a minimum jump distance so an AI doesn't jump if the ball is, say, level with the AI. 
        if (ball)
        {
            if (isJumping == false && Mathf.Abs(ball.transform.position.x - transform.position.x) <= maxHorizontalJumpDistance && Mathf.Abs(ball.transform.position.y - transform.position.y) <= distanceToTriggerJump && Mathf.Abs(ball.transform.position.y - transform.position.y) >= .6f)
            {
                Vector3 jumpForce = new Vector3(0f, jumpPower * rb.gravityScale, 0f);
                rb.AddForce(jumpForce);
                SoundManagerScript.instance.RandomizeSfx(jumpSound1, jumpSound2);
                isJumping = true;
            }
        }
	}

	void CheckForGravChange(){
        if (ball)
        {
            if (ball.GetComponent<Rigidbody2D>().gravityScale > 0)
            {   // TODO: Add an option for 'inverse' or opposite grav as the ball
                rb.gravityScale = 1;
            }
            else
            {
                rb.gravityScale = -1; 
            }
        }
	}

	void checkPenalty(){
		switch (team) {
		case 1:
			if (transform.position.x > -1.0f){
				penaltyTimerActive = true;
				penaltyTimer = 10f;
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				trail.GetComponent<Trail>().ClearSystem (true);
				trail.SetActive (false);
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;


			}
			break;

		case 2:
			if (transform.position.x < 1.0f){
				penaltyTimerActive = true;
				penaltyTimer = 10f;
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				trail.GetComponent<Trail>().ClearSystem (true);
				trail.SetActive (false);
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			}

			break;
		}
	}

	void OnCollisionStay2D(Collision2D collisionInfo) {

		if (collisionInfo.gameObject.tag == "Playfield") {
			//	Debug.Log ("stay with playfield");
			//	GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			canMove = false;
		}
	}
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "ScoringBoundary" || coll.gameObject.tag == "Player") {
			//Debug.Log ("a collision!");
			isJumping = false;
			//	Debug.Log (isJumping);
			SoundManagerScript.instance.PlaySingle (landSound);
		}

		if (coll.gameObject.tag == "Playfield") {
			//	GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			canMove = false;
			//SoundManagerScript.instance.PlaySingle (landSound);
		}

		if (coll.gameObject.tag == "Ball") {
			// update the ball's touch information
			BallScript ball = coll.gameObject.GetComponent<BallScript>();
			ball.secondToLastTouch = ball.lastTouch;
			ball.lastTouch = playerID;

			// check relative velocity of collision
			//			Debug.Log(coll.relativeVelocity.magnitude);
			//			if (coll.relativeVelocity.magnitude > 40) {
			//				SoundManagerScript.instance.PlaySingle (collideWithBallSoundBig);
			//			} else {
			//				SoundManagerScript.instance.RandomizeSfx (collideWithBallSound1, collideWithBallSound2);
			//			}
		}

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Powerup") {
		//	Debug.Log ("Happening");
			//Script other = coll.gameObject.GetComponent<NewPowerUpScript> ();
			int whichPowerup = coll.gameObject.GetComponent<NewPowerUpScript> ().powerupType;
			if (coll.gameObject.GetComponent<NewPowerUpScript> ().isAvailable) {
				coll.gameObject.GetComponent<NewPowerUpScript> ().FadeOut ();
				ApplyPowerup (whichPowerup);
			}
			//Destroy (coll.gameObject);

		}
	}
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "ScoringBoundary" || coll.gameObject.tag == "Player") {
			//Debug.Log ("a collision ended!");
			if (!isJumping) {
				//isJumping = true;
			}
			//Debug.Log (isJumping);
		}

		if (coll.gameObject.tag == "Playfield") {
			//	canMove = true;
		}

		if (coll.gameObject.tag == "Ball") {
		//	Debug.Log (coll.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude);
			var mag = coll.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;
			if (mag > 30) {
				SoundManagerScript.instance.PlaySingle (collideWithBallSoundBig);
			} else {
				SoundManagerScript.instance.RandomizeSfx (collideWithBallSound1, collideWithBallSound2);
			}
		}
	}

	void ClampPosition(){

		// Only clamp position if pandemonium is not active;
		if (!pandemoniumPowerupActive){
			var pos = transform.position;
			if (team == 1) {
				// TODO: Make this dynamic based on raycasting
				pos.x = Mathf.Clamp (transform.position.x, -27.2f, -1.0f);
				transform.position = pos;
			} else if (team == 2) {
				pos.x = Mathf.Clamp (transform.position.x, 1f, 27.2f);
				transform.position = pos;
			}
		}
	}
	void ManagePowerups(){
		if (speedPowerupActive) {
			speedPowerupTimer -= Time.deltaTime;

			if (speedPowerupTimer <= 0){
				speedPowerupActive = false;
				speed = startSpeed;
			}
		}

		if (sizePowerupActive) {
			sizePowerupTimer -= Time.deltaTime;

			if (sizePowerupTimer <= 0){
				sizePowerupActive = false;
				// Restore scale to starting size
				gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
				rb.mass = startMass;
				jumpPower = startJumpPower;

			}
		}

		if (pandemoniumPowerupActive){
			pandemoniumTimer -= Time.deltaTime;
			pandemoniumCounter.GetComponent<TextMesh> ().color = new Vector4(1f, 1f, 1f, .25f);
			pandemoniumCounter.GetComponent<TextMesh> ().text = Mathf.Floor(pandemoniumTimer).ToString();
			if (pandemoniumTimer <= 0) {
				pandemoniumCounter.GetComponent<TextMesh> ().color = new Vector4(0f, 0f, 0f, 0f);
				pandemoniumPowerupActive = false;
				// run 'punishment' check if player is offsides.
				checkPenalty();
			}
		}

		if (penaltyTimerActive) {
			penaltyTimer -= Time.deltaTime;

			if (penaltyTimer <= 0) {
				penaltyTimerActive = false;
				ReturnFromPenalty ();
			}
		}
	}

	void ReturnFromPenalty(){
		switch (team) {
		case 1:
			transform.position = new Vector3 (-6f, 0f, -0.5f);


			break;

		case 2:
			transform.position = new Vector3 (6f, 0f, -0.5f);

			break;
		}
		rb.velocity = Vector3.zero;
		rb.gravityScale = startingGrav;
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;

		trail.SetActive (true);
		trail.GetComponent<Trail>().ClearSystem (true);
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;

	}

	void ApplyPowerup(int whichPowerup){
		//Debug.Log (whichPowerup);
		switch (whichPowerup) {

		case 1:
			speedPowerupActive = true;
			speed = 22f;
			speedPowerupTimer = 20f;

			break;

		case 2:

			sizePowerupActive = true;
			gameObject.transform.localScale = new Vector3 (2f, 2f, 1f);
			rb.mass = startMass * 2f;
			jumpPower = startJumpPower * 1.75f;
			sizePowerupTimer = 20f;

			break;

		case 3:
			pandemoniumPowerupActive = true;
			pandemoniumTimer = 20f;
			break;

		case 4:
			Camera.main.GetComponent<ManageWiggleScript> ().ActivateWiggle ();
			break;
		default:


			break;
		}

	}
}
