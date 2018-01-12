using UnityEngine;

public class Deathzone : MonoBehaviour {

    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.SendMessage("Die");
        }
    }
}