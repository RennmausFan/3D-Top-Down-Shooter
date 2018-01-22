using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private GameObject player;

    public Vector3 offset = new Vector3(0, 0.5f, -1);
    private bool isActivated;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isActivated)
            {
                isActivated = true;
                Vector3 spawnpoint = gameObject.transform.position + offset;
                player.GetComponent<CubeActions>().SetSpawnpoint(spawnpoint);
                print("Checkpoint activated");
            }
        }
    }
}
