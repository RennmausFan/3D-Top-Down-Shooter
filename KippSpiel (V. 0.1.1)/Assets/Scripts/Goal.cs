using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private GameObject player;

    [SerializeField]
    private GameObject goalPopUp;

    private float timer;
    private bool levelCompleted;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (levelCompleted && timer > goalPopUp.GetComponentInChildren<PopUp>().duration)
        {
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().NewLevel();
        }
        timer += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!levelCompleted)
            {
                player.GetComponent<PlayerController>().SetStage(player.GetComponent<PlayerController>().GetStage() + 1);
                Instantiate(goalPopUp, Vector3.zero, Quaternion.identity);
                levelCompleted = true;
                timer = 0f;
            }
        }
    }

}
