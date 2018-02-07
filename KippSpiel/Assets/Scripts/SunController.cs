using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour {

    PlayerController player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.isOutside)
        {
            GetComponent<Light>().intensity = 1f;
        }
        else
        {
            GetComponent<Light>().intensity = 0f;
        }
    }
}
