using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Stage completed!!");
            player.GetComponent<CubeActions>().SetStage(player.GetComponent<CubeActions>().GetStage());
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().NewLevel();
        }
    }

}
