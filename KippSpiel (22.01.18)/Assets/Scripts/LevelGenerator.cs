using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject[] modules;

    [SerializeField]
    private GameObject[] cornerModules;

    [SerializeField]
    private GameObject startModul;
    private GameObject start;

    private int maxRooms;

	// Use this for initialization
	void Awake () {
        start = Instantiate(startModul, Vector3.zero, Quaternion.Euler(0, RandomRotation(),0));
        BuildLevel(start);
	}

    //Recursiv method that creates level by attaching a modul at each door
    void BuildLevel(GameObject modul)
    {
        print("Recursiv bounce!");
        LinkedList<direction> doors = modul.GetComponent<ModulManager>().GetDoors();
        print(doors.Count);
        foreach (direction door in doors)
        {
            if (maxRooms >= 4)
            {
                return;
            }
            if (door == direction.Left)
            {
                print("Left");
                Vector3 pos = modul.transform.position + new Vector3(-30,0,0);
                GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
                ModulManager script = newModul.GetComponent<ModulManager>();
                script.RotateToDoor(script.GetRight());
                maxRooms++;
                BuildLevel(newModul);
            }
            if (door == direction.Right)
            {
                print("Right");
                Vector3 pos = modul.transform.position + new Vector3(30, 0, 0);
                GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
                ModulManager script = newModul.GetComponent<ModulManager>();
                script.RotateToDoor(script.GetLeft());
                maxRooms++;
                BuildLevel(newModul);
            }
            if (door == direction.Up)
            {
                print("Up");
                Vector3 pos = modul.transform.position + new Vector3(0, 0, 30);
                GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
                ModulManager script = newModul.GetComponent<ModulManager>();
                script.RotateToDoor(script.GetDown());
                maxRooms++;
                BuildLevel(newModul);
            }
            if (door == direction.Down)
            {
                print("Down");
                Vector3 pos = modul.transform.position + new Vector3(0, 0, -30);
                GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
                ModulManager script = newModul.GetComponent<ModulManager>();
                script.RotateToDoor(script.GetUp());
                maxRooms++;
                BuildLevel(newModul);
            }
            maxRooms++;
        }
    }
    
    //Returns random modul from selected array
    GameObject RandomModul(GameObject[] modules)
    {
        int random = (int) Random.Range(0f, modules.Length);
        return modules[random];
    }

    //Returns a random rotation, either 0, 90, 180, 270
    float RandomRotation()
    {
        float random = Random.Range(0f,1f);
        if (random <= 0.25f)
        {
            return 0f;
        }
        else if (random > 0.25f && random <= 0.5f)
        {
            return 90f;
        }
        else if (random > 0.5f && random <= 0.75f)
        {
            return 180f;
        }
        else
        {
            return -90f;
        }
    }
}
