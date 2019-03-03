﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManagerScript : MonoBehaviour {

	public GameObject ballPrefab;

    // Store which challenge is being played
    private int currentChallenge;

	// Store reference to challenge-level UI elements
	public GameObject winPanel;
	public GameObject losePanel;
	public GameObject instructionPanel;
	public GameObject challengeTitle;
	public GameObject challengeNumber;
    public GameObject timerText;
    private Text timerTextObj;

	// Store a flag the individual challenge can reference to know whether to start or stop the challenge
	public bool challengeRunning = false;

	// Store a reference to the challenges container so we can activate the correct challenge
	public GameObject challengesContainer;

    // Manage the time of the challenge
    private float rawTimer = 0f;

	// Static singleton property
	public static ChallengeManagerScript Instance { get; private set; }


	void Awake(){
		Instance = this;
		// Load the challenge the user requested
		Debug.Log("Switching to challenge " + DataManagerScript.challengeType);
		SwitchToChallenge(DataManagerScript.challengeType);
        currentChallenge = DataManagerScript.challengeType;
        timerTextObj = timerText.GetComponent<Text>();

    }

	void Start () {
		// Display instruction panel
		DisplayChallengeInstructions();
       
		// For now, just hide the panel in 3 seconds
		Invoke("HideChallengeInstructions", 3f);
	}
	
	void Update () {

        //Update the challenge timer 
        if (challengeRunning)
        {
            rawTimer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(rawTimer / 60F);
            int seconds = Mathf.FloorToInt(rawTimer - minutes * 60);
            float fraction = (rawTimer * 100) % 100;
            string niceTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
            timerTextObj.text = niceTime;
        }
    }

	public void UpdateChallengeText(string newText){
		challengeTitle.GetComponent<Text>().text = newText;
		// TODO: Make a helper function to format the challenge number string
		challengeNumber.GetComponent<Text>().text = "CHALLENGE 0" + (DataManagerScript.challengeType + 1);
	}

	private void SwitchToChallenge(int whichChallenge){
		Transform challenge = challengesContainer.transform.GetChild (whichChallenge);
		challenge.gameObject.SetActive (true);
	}

	public void DisplayChallengeInstructions(){
		instructionPanel.SetActive(true);
	}

	public void HideChallengeInstructions(){
		instructionPanel.SetActive(false);
		challengeRunning = true;
	}

	public void ChallengeFail(){
		// Display fail text
		losePanel.SetActive(true);
        challengeRunning = false;

        //For now, restart the challenge
        Invoke("RestartChallenge", 5f);
    }

    public void RestartChallenge()
    {
        Application.LoadLevel("challengeScene");
    }

    public void PlayNextChallenge()
    {
        DataManagerScript.challengeType = DataManagerScript.challengeType + 1;
        Debug.Log("INCREASED CHALLENGE NUM!");
        Application.LoadLevel("challengeScene");

        ////TODO: Check for last challenge here

        ////Disable current challenge
        //Transform challenge = challengesContainer.transform.GetChild(currentChallenge);
        //challenge.gameObject.SetActive(false);

        ////Switch to new challenge
        //winPanel.SetActive(false);
        //currentChallenge += 1;
        //SwitchToChallenge(currentChallenge);

        //// Display instruction panel
        //DisplayChallengeInstructions();

        //// For now, just hide the panel in 3 seconds
        //Invoke("HideChallengeInstructions", 3f);
    }

    public void ChallengeSucceed(){

		// Display success text
		winPanel.SetActive(true);
        challengeRunning = false;


        //For now, try playing the next challenge
        Invoke("PlayNextChallenge", 5f);
  

    }
}
