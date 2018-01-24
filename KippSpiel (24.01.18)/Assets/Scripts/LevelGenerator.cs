using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject[] modules;

    [SerializeField]
    private GameObject[] cornerModules;

    [SerializeField]
    private GameObject startModul;
    private GameObject start;

    public int maxRoomDepth;
    private int roomDepth;

    // Use this for initialization
    void Awake()
    {
        start = Instantiate(startModul, Vector3.zero, Quaternion.Euler(0, RandomRotation(), 0));
        BuildLevel(start, direction.None);
    }

    //Recursiv method that creates level by attaching a modul at each door
    void BuildLevel(GameObject modul, direction ignoreDirection)
    {
        print(roomDepth);
        if (roomDepth >= maxRoomDepth)
        {
            return;
        }
        LinkedList<direction> doors = modul.GetComponent<ModulManager>().GetDoors();
        if (doors.Contains(direction.Left) && ignoreDirection != direction.Left)
        {
            Vector3 pos = modul.transform.position + new Vector3(-30, 0, 0);
            GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
            ModulManager script = newModul.GetComponent<ModulManager>();
            script.RotateToDoor(direction.Right);
            BuildLevel(newModul, direction.Right);
            //roomDepth++;
        }
        if (doors.Contains(direction.Right) && ignoreDirection != direction.Right)
        {
            Vector3 pos = modul.transform.position + new Vector3(30, 0, 0);
            GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
            ModulManager script = newModul.GetComponent<ModulManager>();
            script.RotateToDoor(direction.Left);
            BuildLevel(newModul, direction.Left);
            //roomDepth++;
        }
        if (doors.Contains(direction.Up) && ignoreDirection != direction.Up)
        {
            Vector3 pos = modul.transform.position + new Vector3(0, 0, 30);
            GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
            ModulManager script = newModul.GetComponent<ModulManager>();
            script.RotateToDoor(direction.Down);
            BuildLevel(newModul, direction.Down);
            //roomDepth++;
        }
        if (doors.Contains(direction.Down) && ignoreDirection != direction.Down)
        {
            Vector3 pos = modul.transform.position + new Vector3(0, 0, -30);
            GameObject newModul = Instantiate(RandomModul(modules), pos, Quaternion.identity);
            ModulManager script = newModul.GetComponent<ModulManager>();
            script.RotateToDoor(direction.Up);
            BuildLevel(newModul, direction.Up);
            //roomDepth++;
        }
        roomDepth++;
    }

    //Returns random modul from selected array
    GameObject RandomModul(GameObject[] modules)
    {
        int random = (int)Random.Range(0f, modules.Length);
        return modules[random];
    }

    //Returns a random rotation, either 0, 90, 180, 270
    float RandomRotation()
    {
        float random = Random.Range(0f, 1f);
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
