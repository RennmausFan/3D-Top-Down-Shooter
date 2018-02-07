using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int stage = PlayerPrefs.GetInt("highscoreStage",0);
        int glow = PlayerPrefs.GetInt("highscoreGlow", 0);
        string date = PlayerPrefs.GetString("highscoreDate", "");
        GetComponent<Text>().text = date + "\t" + "Stage: " + stage + ".  " + "Glow: " + glow;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
