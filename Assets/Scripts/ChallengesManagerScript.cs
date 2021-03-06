﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengesManagerScript : MonoBehaviour {

    private int markerPos = 0;
    private float[] markerYPositions = { 7.4f, 3.7f, 0f, -3.7f, -7.4f };
    private float[] markerXPositions = { -17.58f, -1.81f };
	private int joystickIdentifier = 0;
    public GameObject marker;
    private bool axis1InUse = false;
    private bool axis2InUse = false;
    private bool axis3InUse = false;
    private bool axis4InUse = false;
    private int numberOfChallenges = 9;
    private bool locked = false;
    public AudioClip tickUp;
    public AudioClip tickDown;

    private new AudioSource audio;

	vAxis verticalAxis;
	vAxis horizontalAxis;
    private EventSystem es;
    public JoystickButtons buttons;

    // Use this for initialization
    void Start()
    {
		// Fade in
		GameObject curtain = GameObject.Find("FadeCurtainCanvas");
        LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 0f, .5f);

        audio = GetComponent<AudioSource>();
        locked = false;
        MusicManagerScript.Instance.FadeOutEverything();
        // Assign joystick to player
        joystickIdentifier = DataManagerScript.gamepadControllingMenus;
        buttons = new JoystickButtons(joystickIdentifier);

        // Get axis string from joystick class
        verticalAxis = new vAxis(buttons.vertical);
		horizontalAxis = new vAxis(buttons.horizontal);

        Vector3 tempPos = new Vector3(markerXPositions[0], markerYPositions[0], 1f);
        marker.transform.position = tempPos;
        // Get current event system and null out
        es = EventSystem.current;
        Debug.Log("what is button horiz");
        Debug.Log(buttons.horizontal);
        es.GetComponent<StandaloneInputModule>().horizontalAxis = buttons.horizontal;
        es.GetComponent<StandaloneInputModule>().verticalAxis = buttons.vertical;
        es.GetComponent<StandaloneInputModule>().submitButton = buttons.jump;
        es.GetComponent<StandaloneInputModule>().cancelButton = buttons.grav;

        Debug.Log("Achievement manager?");
        Debug.Log(AchievementManagerScript.Instance);
        if (AchievementManagerScript.Instance != null)
        {

            // Report progress to achievement manager
            AchievementManagerScript.Instance.LogMedalProgress(HowManyMedals(), HowManyGoldMedals());
            Debug.Log("counting medals");
            if (IsAllMedals())
            {
                AchievementManagerScript.Instance.Achievements[8].Unlock();
            }
            if (IsAllGoldMedals())
            {
                AchievementManagerScript.Instance.Achievements[7].Unlock();
            }
        }
    }

    void Update()
    {
        if (!locked){

                if (Input.GetAxisRaw(verticalAxis.axisName) < 0){
                    if (verticalAxis.axisInUse == false)
                    {
                        // Call your event function here.
                        verticalAxis.axisInUse = true;
                        markerPos++;
                        updateMarkerPos();
                        audio.PlayOneShot(tickUp);
                    }
                }

                if (Input.GetAxisRaw(verticalAxis.axisName) > 0)
                {
                    if (verticalAxis.axisInUse == false)
                    {
                        // Call your event function here.
                        verticalAxis.axisInUse = true;
                        markerPos--;
                        audio.PlayOneShot(tickDown);
                        updateMarkerPos();
                    }
                }

                if (Input.GetAxisRaw(verticalAxis.axisName) == 0)
                {
                    verticalAxis.axisInUse = false;
                }



                if (Input.GetAxisRaw(horizontalAxis.axisName) < 0)
                {
				if (horizontalAxis.axisInUse == false)
                    {
                        // Call your event function here.
					horizontalAxis.axisInUse = true;
                        markerPos += 5;
                        updateMarkerPos();
                        audio.PlayOneShot(tickUp);

                    }
                }

			if (Input.GetAxisRaw(horizontalAxis.axisName) > 0)
                {
				if (horizontalAxis.axisInUse == false)
                    {
                        // Call your event function here.
					horizontalAxis.axisInUse = true;
                        markerPos += 5;
                        audio.PlayOneShot(tickDown);
                        updateMarkerPos();
                    }
                }

			if (Input.GetAxisRaw(horizontalAxis.axisName) == 0)
                {
				horizontalAxis.axisInUse = false;
                }

            {
				if (Input.GetButtonDown(buttons.jump))
                {
                    // Set chosen challenge
                    DataManagerScript.challengeType = markerPos;
                    NextScene();
                }
            }
        }
    }

    void NextScene()
    {
        if (!locked) {
            locked = true;
			GameObject curtain = GameObject.Find("FadeCurtainCanvas");
            LeanTween.alpha(curtain.GetComponentInChildren<Image>().rectTransform, 1f, .5f).setOnComplete(() => { SceneManager.LoadSceneAsync("challengeScene"); });
        }
    }

    string formatTime(float rawTime)
    {
        int minutes = Mathf.FloorToInt(rawTime / 60F);
        int seconds = Mathf.FloorToInt(rawTime - minutes * 60);
        float fraction = (rawTime * 100) % 100;
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
        return niceTime;
    }

    void updateMarkerPos()
    {

        if (markerPos < 0)
        {
            markerPos = numberOfChallenges;
        }
        markerPos = markerPos % (numberOfChallenges + 1);
        float posX;
        float posY;

        if (markerPos > 4)
        {
            posX = markerXPositions[1];
            marker.transform.localScale = new Vector3(-3f, marker.transform.localScale.y, marker.transform.localScale.z);
            posY = markerYPositions[markerPos - 5];

        }
        else
        {
            posX = markerXPositions[0];
            marker.transform.localScale = new Vector3(3f, marker.transform.localScale.y, marker.transform.localScale.z);
            posY = markerYPositions[markerPos];
        }

        Vector3 tempPos = new Vector3(posX, posY, 1f);
        marker.transform.position = tempPos;
    }

    public int HowManyMedals()
    {
        int howMany = 0;
        for (int i = 0; i < 10; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i);
            medalTypes whichMedal = mp.GetMedal();
            if (whichMedal != medalTypes.none)
            {
                howMany++;
            }
        }
        return howMany;
    }

    public int HowManyGoldMedals()
    {
        int howMany = 0;
        for (int i = 0; i < 10; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i);
            medalTypes whichMedal = mp.GetMedal();
            if (whichMedal == medalTypes.gold)
            {
                howMany++;
            }
        }
        return howMany;
    }

    public bool IsAllMedals()
    {
        Debug.Log("running all medals");
        for (int i = 0; i < 10; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i);
            medalTypes whichMedal = mp.GetMedal();
            Debug.Log("medal?");
            Debug.Log(whichMedal);
            if (whichMedal == medalTypes.none)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsAllGoldMedals()
    {

        for (int i = 0; i < 10; i++)
        {
            float bestTime = FileSystemLayer.Instance.GetChallengeTime(i);
            MedalProvider mp = new MedalProvider(bestTime, i);
            medalTypes whichMedal = mp.GetMedal();
            if (whichMedal != medalTypes.gold)
            {
                return false;
            }
        }

        return true;
    }
}