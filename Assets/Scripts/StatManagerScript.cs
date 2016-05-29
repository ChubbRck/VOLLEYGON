﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class StatManagerScript : MonoBehaviour {

	public GameObject MVPBackground;
	public Text player1Aces;
	public Text player2Aces;
	public Text player3Aces;
	public Text player4Aces;

	public Text player1Scores;
	public Text player2Scores;
	public Text player3Scores;
	public Text player4Scores;

	public Text player1Returns;
	public Text player2Returns;
	public Text player3Returns;
	public Text player4Returns;

	public Text player1Bumbles;
	public Text player2Bumbles;
	public Text player3Bumbles;
	public Text player4Bumbles;

	public Text player1Labels;
	public Text player2Labels;
	public Text player3Labels;
	public Text player4Labels;

	public Text player1Title;
	public Text player2Title;
	public Text player3Title;
	public Text player4Title;

	public Text Player1MVP;
	public Text Player2MVP;
	public Text Player3MVP;
	public Text Player4MVP;

	//private int[] aces = { 1, 2, 3, 4, 5, 6, 7 };
	public int[] aces;
	private int[] scores;
	private int[] returns;
	private int[] bumbles;

	public int playersPlaying = 0;
	public int playersReady = 0;

	private float scoreWeight = 1f;
	private float aceWeight = 1f;
	private float returnWeight = .25f;
	private float bumbleWeight = .5f;

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;


//	nums.Max(); // Will result in 7
//	nums.Min(); // Will result in 1

	public static StatManagerScript Instance { get; private set; }
	// Use this for initialization

	void Awake(){
		Instance = this;
	}

	void Start () {

		Player1MVP.GetComponent<CanvasRenderer> ().SetAlpha (0f);
		Player2MVP.GetComponent<CanvasRenderer> ().SetAlpha (0f);
		Player3MVP.GetComponent<CanvasRenderer> ().SetAlpha (0f);
		Player4MVP.GetComponent<CanvasRenderer> ().SetAlpha (0f);

		PopulateStats ();

		// Determine how many players are playing.
		if (DataManagerScript.playerOnePlaying) {
			playersPlaying++; 
		}
		if (DataManagerScript.playerTwoPlaying) {
			playersPlaying++; 
		}
		if (DataManagerScript.playerThreePlaying) {
			playersPlaying++; 
		}
		if (DataManagerScript.playerFourPlaying) {
			playersPlaying++; 
		}

		Invoke ("BackToTitle", 30f);

	}
	
	// Update is called once per frame

	void Update () {
		if (playersReady == playersPlaying) {
			BackToTitle ();
		}
	}

	public void CheckStartable(){
		
	}

	public void increasePlayerReady(){
		playersReady++;

	}

	public void decreasePlayerReady(){
		playersReady--;
	}

	void DetermineMVP(){
		float p1Score = DataManagerScript.playerOneAces * aceWeight + DataManagerScript.playerOneScores * scoreWeight + DataManagerScript.playerOneReturns * returnWeight - DataManagerScript.playerOneBumbles * bumbleWeight; 
		float p2Score = DataManagerScript.playerTwoAces * aceWeight + DataManagerScript.playerTwoScores * scoreWeight + DataManagerScript.playerTwoReturns * returnWeight - DataManagerScript.playerTwoBumbles * bumbleWeight; 
		float p3Score = DataManagerScript.playerThreeAces * aceWeight + DataManagerScript.playerThreeScores * scoreWeight + DataManagerScript.playerThreeReturns * returnWeight - DataManagerScript.playerThreeBumbles * bumbleWeight; 
		float p4Score = DataManagerScript.playerFourAces * aceWeight + DataManagerScript.playerFourScores * scoreWeight + DataManagerScript.playerFourReturns * returnWeight - DataManagerScript.playerFourBumbles * bumbleWeight; 
	
		float[] scores = { p1Score, p2Score, p3Score, p4Score };

		if (p1Score == scores.Max ()) {
			activateMVP (1);
		}

		if (p2Score == scores.Max ()) {
			activateMVP (2);
		}

		if (p3Score == scores.Max ()) {
			activateMVP (3);
		}

		if (p4Score == scores.Max ()) {
			activateMVP (4);
		}
	}

	void activateMVP(int whichPlayer){
		switch (whichPlayer) {
		case 1:
			//MVPBackground.transform.position = new Vector3 (-13.32f, -0.07f, 100f);
			Instantiate(MVPBackground, new Vector3 (-13.32f, -0.07f, 100f), Quaternion.identity);
			Player1MVP.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			break;
		case 2:
			//MVPBackground.transform.position = new Vector3 (-3.32f, -0.07f, 100f);
			Instantiate(MVPBackground, new Vector3 (-3.32f, -0.07f, 100f), Quaternion.identity);
			Player2MVP.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			break;
		case 3:
			//MVPBackground.transform.position = new Vector3 (6.32f, -0.07f, 100f);
			Instantiate(MVPBackground, new Vector3 (6.32f, -0.07f, 100f), Quaternion.identity);
			Player3MVP.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			break;
		case 4:
			//MVPBackground.transform.position = new Vector3 (16.32f, -0.07f, 100f);
			Instantiate(MVPBackground, new Vector3 (16.32f, -0.07f, 100f), Quaternion.identity);
			Player4MVP.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
			break;

		}
	}

	void BackToTitle(){
		Application.LoadLevel ("titleScene");
	}

	void PopulateStats(){

		int[] aces = {
			DataManagerScript.playerOneAces,
			DataManagerScript.playerTwoAces,
			DataManagerScript.playerThreeAces,
			DataManagerScript.playerFourAces
		};

		int[] scores = {
			DataManagerScript.playerOneScores,
			DataManagerScript.playerTwoScores,
			DataManagerScript.playerThreeScores,
			DataManagerScript.playerFourScores
		};

		int[] returns = {
			DataManagerScript.playerOneReturns,
			DataManagerScript.playerTwoReturns,
			DataManagerScript.playerThreeReturns,
			DataManagerScript.playerFourReturns
		};

		int[] bumbles = {
			DataManagerScript.playerOneBumbles,
			DataManagerScript.playerTwoBumbles,
			DataManagerScript.playerThreeBumbles,
			DataManagerScript.playerFourBumbles
		};


		// if player active...
		if (DataManagerScript.playerOnePlaying) {
			player1Aces.text = "ACES: " + DataManagerScript.playerOneAces.ToString (); 
			player1Scores.text = "SCORES: " + DataManagerScript.playerOneScores.ToString ();
			player1Returns.text = "RETURNS: " + DataManagerScript.playerOneReturns.ToString ();
			player1Bumbles.text = "BUMBLES: " + DataManagerScript.playerOneBumbles.ToString ();

			if (DataManagerScript.playerOneAces == aces.Max()){
				player1Aces.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerOneReturns == returns.Max()){
				player1Returns.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerOneScores == scores.Max()){
				player1Scores.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerOneBumbles == bumbles.Max()){
				player1Bumbles.color = HexToColor("d82039");
			}

		} else {
			player1Aces.text = "ACES: " + DataManagerScript.playerFourAces.ToString (); 
			player1Scores.text = "SCORES: " + DataManagerScript.playerFourScores.ToString ();
			player1Returns.text = "RETURNS: " + DataManagerScript.playerFourReturns.ToString ();
			player1Bumbles.text = "BUMBLES: " + DataManagerScript.playerFourBumbles.ToString ();
			player1Aces.color = HexToColor ("000000");
			player1Scores.color = HexToColor ("000000");
			player1Returns.color = HexToColor ("000000");
			player1Bumbles.color = HexToColor ("000000");
			player1Title.color = HexToColor ("000000");
//			player1Title.enabled = false; 
//			player1Labels.enabled = false;
		}

		if (DataManagerScript.playerTwoPlaying) {
			player2Aces.text = "ACES: " + DataManagerScript.playerTwoAces.ToString (); 
			player2Scores.text = "SCORES: " + DataManagerScript.playerTwoScores.ToString ();
			player2Returns.text = "RETURNS: " + DataManagerScript.playerTwoReturns.ToString ();
			player2Bumbles.text = "BUMBLES: " + DataManagerScript.playerTwoBumbles.ToString ();

			if (DataManagerScript.playerTwoAces == aces.Max()){
				player2Aces.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerTwoReturns == returns.Max()){
				player2Returns.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerTwoScores == scores.Max()){
				player2Scores.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerTwoBumbles == bumbles.Max()){
				player2Bumbles.color = HexToColor("d82039");
			}

		} else {
			player2Aces.text = "ACES: " + DataManagerScript.playerFourAces.ToString (); 
			player2Scores.text = "SCORES: " + DataManagerScript.playerFourScores.ToString ();
			player2Returns.text = "RETURNS: " + DataManagerScript.playerFourReturns.ToString ();
			player2Bumbles.text = "BUMBLES: " + DataManagerScript.playerFourBumbles.ToString ();
			player2Aces.color = HexToColor ("000000");
			player2Scores.color = HexToColor ("000000");
			player2Returns.color = HexToColor ("000000");
			player2Bumbles.color = HexToColor ("000000");
			player2Title.color = HexToColor ("000000");
			//player2Title.enabled = false; 
			//player2Labels.enabled = false;
		}

		if (DataManagerScript.playerThreePlaying) {
			player3Aces.text = "ACES: " + DataManagerScript.playerThreeAces.ToString (); 
			player3Scores.text = "SCORES: " + DataManagerScript.playerThreeScores.ToString ();
			player3Returns.text = "RETURNS: " + DataManagerScript.playerThreeReturns.ToString ();
			player3Bumbles.text = "BUMBLES: " + DataManagerScript.playerThreeBumbles.ToString ();

			if (DataManagerScript.playerThreeAces == aces.Max()){
				player3Aces.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerThreeReturns == returns.Max()){
				player3Returns.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerThreeScores == scores.Max()){
				player3Scores.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerThreeBumbles == bumbles.Max()){
				player3Bumbles.color = HexToColor("d82039");
			}

		} else {
//			player3Title.enabled = false; 
//			player3Labels.enabled = false;
			player3Aces.text = "ACES: " + DataManagerScript.playerFourAces.ToString (); 
			player3Scores.text = "SCORES: " + DataManagerScript.playerFourScores.ToString ();
			player3Returns.text = "RETURNS: " + DataManagerScript.playerFourReturns.ToString ();
			player3Bumbles.text = "BUMBLES: " + DataManagerScript.playerFourBumbles.ToString ();
			player3Aces.color = HexToColor ("000000");
			player3Scores.color = HexToColor ("000000");
			player3Returns.color = HexToColor ("000000");
			player3Bumbles.color = HexToColor ("000000");
			player3Title.color = HexToColor ("000000");

		}

		if (DataManagerScript.playerFourPlaying) {
			player4Aces.text = "ACES: " + DataManagerScript.playerFourAces.ToString (); 
			player4Scores.text = "SCORES: " + DataManagerScript.playerFourScores.ToString ();
			player4Returns.text = "RETURNS: " + DataManagerScript.playerFourReturns.ToString ();
			player4Bumbles.text = "BUMBLES: " + DataManagerScript.playerFourBumbles.ToString ();

			if (DataManagerScript.playerFourAces == aces.Max()){
				player4Aces.color = HexToColor ("ffb752");

			}

			if (DataManagerScript.playerFourReturns == returns.Max()){
				player4Returns.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerFourScores == scores.Max()){
				player4Scores.color = HexToColor ("ffb752");
			}

			if (DataManagerScript.playerFourBumbles == bumbles.Max()){
				player4Bumbles.color = HexToColor("d82039");
			}

		} else {
			//player4Title.enabled = false; 
			//player4Labels.enabled = false;
			player4Aces.text = "ACES: " + DataManagerScript.playerFourAces.ToString (); 
			player4Scores.text = "SCORES: " + DataManagerScript.playerFourScores.ToString ();
			player4Returns.text = "RETURNS: " + DataManagerScript.playerFourReturns.ToString ();
			player4Bumbles.text = "BUMBLES: " + DataManagerScript.playerFourBumbles.ToString ();
			player4Aces.color = HexToColor ("000000");
			player4Scores.color = HexToColor ("000000");
			player4Returns.color = HexToColor ("000000");
			player4Bumbles.color = HexToColor ("000000");
			player4Title.color = HexToColor ("000000");
		}

		DetermineMVP ();
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
}