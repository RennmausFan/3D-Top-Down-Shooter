using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPlayer : MonoBehaviour {

    public int glow;

    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        glow = player.GetComponent<CubeActions>().GetGlow();
        //Set glow of the player to zero
        player.GetComponent<CubeActions>().SetGlow(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.SendMessage("AddGlow",glow);
            Destroy(gameObject);
        }       
    }
}
