using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    
    // Use this for initialization
	void Start () {
        Vector3 spawnpoint = player.GetComponent<CubeActions>().GetSpawnpoint();
        player.transform.position = spawnpoint;
	}

    //Triggered if player dies
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
