using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject player;
    private CubeActions _CubeActions;

    [SerializeField]
    private GameObject glowPlayer;

    [SerializeField]
    private GameObject stagePopUp;

    // Use this for initialization
    void Start()
    {
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
        GameObject go = Instantiate(stagePopUp, Vector3.zero, Quaternion.identity);
        GameObject uIManager = GameObject.FindWithTag("UIManager");
        go.transform.SetParent(uIManager.transform);
    }

    //Triggered if player dies
    public void RestartGame()
    {
        //Spawn GlowPlayer at the death location of the player
        if (_CubeActions.GetGlow() > 0)
        {
            Vector3 deathpoint = _CubeActions.GetDeathpoint();
            Instantiate(glowPlayer, deathpoint, Quaternion.identity);
        }
        //Respawn player
        Vector3 spawnpoint = _CubeActions.GetSpawnpoint();
        player.transform.position = spawnpoint;
        _CubeActions.health = 100;
    }

    public void NewLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
