using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject player;
    private PlayerController _CubeActions;

    [SerializeField]
    private GameObject glowPlayer;

    [SerializeField]
    private GameObject stagePopUp;

    // Use this for initialization
    void Start()
    {
        //Get Script "CubeActions" from player
        player = GameObject.FindWithTag("Player");
        _CubeActions = player.GetComponent<PlayerController>();

        Vector3 spawnpoint = _CubeActions.GetSpawnpoint();
        player.transform.position = spawnpoint;
        //Spawn GlowPlayer at the death location of the player
        if (_CubeActions.GetGlow() > 0)
        {
            Vector3 deathpoint = _CubeActions.GetDeathpoint();
            Instantiate(glowPlayer, deathpoint, Quaternion.identity);
        }
        Instantiate(stagePopUp, Vector3.zero, Quaternion.identity);
    }

    //Triggered if player dies
    public void RestartGame()
    {
        /*
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
        */
        int stageCount = player.GetComponent<PlayerController>().GetStage();
        int glow = player.GetComponent<PlayerController>().GetGlow();
        int highscoreGlow = PlayerPrefs.GetInt("highscoreGlow", 0);
        int highscoreStage = PlayerPrefs.GetInt("highscoreStage", 0);
        print(System.DateTime.UtcNow);
        if (stageCount >= highscoreStage)
        {
            if (glow >= highscoreGlow)
            {
                PlayerPrefs.SetInt("highscoreStage", stageCount);
                PlayerPrefs.SetInt("highscoreGlow", glow);
                PlayerPrefs.SetString("highscoreDate", System.DateTime.UtcNow.ToShortDateString());
            }
        }
        player.GetComponent<PlayerController>().SetStage(1);
        SceneManager.LoadScene("mainmenu");
    }

    public void NewLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
