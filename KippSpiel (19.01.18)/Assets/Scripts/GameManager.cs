using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject player;
    private CubeActions _CubeActions;

    [SerializeField]
    private GameObject glowPlayer;

    // Use this for initialization
	void Start () {
        //Get Script "CubeActions" from player
        player = GameObject.FindWithTag("Player");
        _CubeActions = player.GetComponent<CubeActions>();

        Vector3 spawnpoint = _CubeActions.GetSpawnpoint();
        player.transform.position = spawnpoint;
        //Spawn GlowPlayer at the death location of the player
        if (_CubeActions.GetGlow() > 0)
        {
            Vector3 deathpoint = _CubeActions.GetDeathpoint();
            Instantiate(glowPlayer, deathpoint, Quaternion.identity);
        }
    }

    //Triggered if player dies
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
