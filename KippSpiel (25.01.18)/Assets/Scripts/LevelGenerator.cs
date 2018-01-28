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

    public int minModules;
    public int maxModules;

    [SerializeField]
    private int modulCount;

    // Use this for initialization
    void Awake()
    {
        start = Instantiate(startModul, Vector3.zero, Quaternion.Euler(0, RandomRotation(), 0));
        BuildLevel(start, direction.None);
    }

    //Recursiv method that creates level by attaching a modul at each door
    void BuildLevel(GameObject modul, direction ignoreDirection)
    {
        GameObject[] selectModules = modules;
        //When maxDepth is reached only create corner modules
        if (modulCount >= maxModules)
        {
            selectModules = cornerModules;
        }
        print("left" + CheckForModul(new Vector3(-30, 0, 0)));
        print("right" + CheckForModul(new Vector3(30, 0, 0)));
        print("up" + CheckForModul(new Vector3(0, 0, 30)));
        print("down" + CheckForModul(new Vector3(0, 0, -30)));


        LinkedList<direction> doors = modul.GetComponent<ModulManager>().GetDoors();

        if (doors.Contains(direction.Left) && ignoreDirection != direction.Left)
        {
            Vector3 pos = modul.transform.position + new Vector3(-30, 0, 0);
            CreateModul(RandomModul(selectModules), pos, direction.Right);
        }
        if (doors.Contains(direction.Right) && ignoreDirection != direction.Right)
        {
            Vector3 pos = modul.transform.position + new Vector3(30, 0, 0);
            CreateModul(RandomModul(selectModules), pos, direction.Left);
        }
        if (doors.Contains(direction.Up) && ignoreDirection != direction.Up)
        {
            Vector3 pos = modul.transform.position + new Vector3(0, 0, 30);
            CreateModul(RandomModul(selectModules), pos, direction.Down);
        }
        if (doors.Contains(direction.Down) && ignoreDirection != direction.Down)
        {
            Vector3 pos = modul.transform.position + new Vector3(0, 0, -30);
            CreateModul(RandomModul(selectModules), pos, direction.Up);
        }
    }
    
    //Instantiate modul
    void CreateModul(GameObject modul, Vector3 pos, direction oppositDirection)
    {
        GameObject newModul = Instantiate(modul, pos, Quaternion.identity);
        ModulManager script = newModul.GetComponent<ModulManager>();
        script.RotateToDoor(oppositDirection);
        modulCount++;
        //Recursiv call
        BuildLevel(newModul, oppositDirection);
    }

    //Check if theres a modul at a specific position
    bool CheckForModul(Vector3 pos)
    {
        GameObject[] modules = GameObject.FindGameObjectsWithTag("Modul");
        foreach (GameObject modul in modules)
        {
            if (modul.transform.position == pos)
            {
                return true;
            }
        }
        return false;
    }

    //Returns modul at specific positionb
    GameObject GetModulAtPosition(Vector3 pos)
    {
        GameObject[] modules = GameObject.FindGameObjectsWithTag("Modul");
        foreach (GameObject modul in modules)
        {
            if (modul.transform.position == pos)
            {
                return modul;
            }
        }
        return null;
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
