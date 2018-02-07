using UnityEngine;
using UnityEngine.UI;

public class Healthdisplay : MonoBehaviour {

    private GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        int health = player.GetComponent<PlayerController>().GetHealth();
        if (health < 0)
        {
            health = 0;
        }
        GetComponent<Text>().text = health.ToString();
    }
}
