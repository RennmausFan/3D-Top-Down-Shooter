using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject player;

    [SerializeField]
    private GameObject stagePopUp;

    public float deathTime;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        Instantiate(stagePopUp, Vector3.zero, Quaternion.identity);
    }

    //Triggered if player dies
    public IEnumerator RestartGame()
    {
        int stageCount = player.GetComponent<PlayerController>().GetStage();
        int glow = player.GetComponent<PlayerController>().GetGlow();
        int highscoreGlow = PlayerPrefs.GetInt("highscoreGlow", 0);
        int highscoreStage = PlayerPrefs.GetInt("highscoreStage", 0);
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
        player.GetComponent<PlayerController>().SetGlow(0);
        yield return new WaitForSeconds(deathTime);
        SceneManager.LoadScene("mainmenu");
    }

    public void NewLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Find the room an object is currently in
    public static GameObject GetRoom(GameObject obj)
    {
        GameObject[] modules = LevelGenerator.GetAllModules();

        foreach (GameObject modul in modules)
        {
            Vector3 modulPos = modul.transform.position;
            Vector3 objPos = obj.transform.position;

            if (objPos.x < (modulPos.x + 15) && objPos.x > (modulPos.x - 15) && objPos.z < (modulPos.z + 15) && objPos.z > (modulPos.z - 15))
            {
                return modul;
            }
        }
        return null;
    }

}
