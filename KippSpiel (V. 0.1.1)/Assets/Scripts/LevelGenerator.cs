using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject[] modules;

    [SerializeField]
    private GameObject[] outsideModules;

    [SerializeField]
    private GameObject[] cornerModules;

    [SerializeField]
    private GameObject startModul;
    private GameObject start;

    [SerializeField]
    private GameObject goalModul;

    private static int maxModules  = 3;

    [SerializeField]
    private int modulCount = 1;

    // Use this for initialization
    void Awake()
    {
        //Increases maxModules by stage
        int stage = GameObject.FindWithTag("Player").GetComponent<PlayerController>().GetStage();
        //Every fitht stage 
        if (stage % 5 == 0)
        {
            maxModules++;
            print(maxModules);
        }

        start = Instantiate(startModul, Vector3.zero, Quaternion.Euler(0, RandomRotation(), 0));
        BuildLevel(start, direction.None);
        //Fix level
        //FixLevel();
        //Create Goal
        GameObject[] corners = GameObject.FindGameObjectsWithTag("Corner");
        GameObject goalDestination = RandomModul(corners);
        Vector3 pos = goalDestination.transform.position;
        Quaternion rotation = goalDestination.transform.rotation;
        Destroy(goalDestination);
        Instantiate(goalModul, pos, rotation);
    }

    //Recursiv method that creates level by attaching a modul at each door
    void BuildLevel(GameObject modul, direction ignoreDirection)
    {
        GameObject[] selectModules = modules;
        ModulManager scriptModul = modul.GetComponent<ModulManager>();
        //When maxDepth is reached only create corner modules
        if (modulCount >= maxModules)
        {
            selectModules = cornerModules;
        }

        LinkedList<direction> doors = modul.GetComponent<ModulManager>().GetDoors();

        if (doors.Contains(direction.Left) && ignoreDirection != direction.Left && !CheckForModul(scriptModul.posLeft))
        {
            CreateModul(RandomModul(selectModules), scriptModul.posLeft, direction.Right);
        }
        if (doors.Contains(direction.Right) && ignoreDirection != direction.Right && !CheckForModul(scriptModul.posRight))
        {
            CreateModul(RandomModul(selectModules), scriptModul.posRight, direction.Left);
        }
        if (doors.Contains(direction.Up) && ignoreDirection != direction.Up && !CheckForModul(scriptModul.posUp))
        {
            CreateModul(RandomModul(selectModules), scriptModul.posUp, direction.Down);
        }
        if (doors.Contains(direction.Down) && ignoreDirection != direction.Down && !CheckForModul(scriptModul.posDown))
        {
            CreateModul(RandomModul(selectModules), scriptModul.posDown, direction.Up);
        }
    }
    
    void FixLevel()
    {
        GameObject[] modules = GetAllModules();
        foreach (GameObject modul in modules)
        {
            state left, right, up, down;
            ModulManager script = modul.GetComponent<ModulManager>();

            //Left
            if (!CheckForModul(script.posLeft))
            {
                left = state.Empty;
            }
            else
            {
                left = GetModulAtPosition(script.posLeft).GetComponent<ModulManager>().GetRight();
            }

            //Right
            if (!CheckForModul(script.posRight))
            {
                right = state.Empty;
            }
            else
            {
                right = GetModulAtPosition(script.posRight).GetComponent<ModulManager>().GetLeft();
            }

            //Up
            if (!CheckForModul(script.posUp))
            {
                up = state.Empty;
            }
            else
            {
                up = GetModulAtPosition(script.posUp).GetComponent<ModulManager>().GetDown();
            }

            //Down
            if (!CheckForModul(script.posDown))
            {
                down = state.Empty;
            }
            else
            {
                down = GetModulAtPosition(script.posDown).GetComponent<ModulManager>().GetUp();
            }
            

            if (!CheckIfModulIsValid(modul, left, right, up, down))
            {
                print("ERROR!!");
                ReplaceModul(modul, left, right, up, down);
            }
        }
    }

    void ReplaceModul(GameObject modul, state left, state right, state up, state down)
    {
        bool modulFound = false;
        Vector3 pos = modul.transform.position;
        for (int z=0; z < modules.Length; z++)
        {
            GameObject newModul = Instantiate(modules[z], pos, Quaternion.identity);
            for (int i=0; i < 4; i++)
            {
                if (CheckIfModulIsValid(newModul, left, right, up, down))
                {
                    print("Found");
                    modulFound = true;
                    break;
                }
                newModul.transform.Rotate(0, 90, 0);

            }
            if (modulFound)
            {
                break;
            }
            Destroy(newModul);
        }
        Destroy(modul);
    }

    //Check for modul a if all doors have a partner from modul b
    bool CheckIfModulIsValid(GameObject modul, state left, state right, state up, state down)
    {
        ModulManager script = modul.GetComponent<ModulManager>();
        state mLeft = script.GetLeft();
        state mRight = script.GetRight();
        state mUp = script.GetUp();
        state mDown = script.GetDown();
        if (CompareStates(mLeft, left) && CompareStates(mLeft, left) && CompareStates(mLeft, left) && CompareStates(mLeft, left))
        {
            return true;
        }
        return false;
    }

    bool CompareStates(state a, state b)
    {
        //Doors fit perfectly --> true
        if (a == state.Door && b == state.Door)
        {
            return true;
        }
        //Only one is a door --> false
        if (a == state.Door || b == state.Door)
        {
            return false;
        }
        //No doors --> true
        return true;
    }

    //Instantiate modul
    void CreateModul(GameObject modul, Vector3 pos, direction oppositDirection)
    {
        GameObject newModul = Instantiate(modul, pos, Quaternion.identity);
        ModulManager script = newModul.GetComponent<ModulManager>();
        script.RotateToDoor(oppositDirection);
        modulCount++;
        BuildLevel(newModul, oppositDirection);
    }

    //Check if theres a modul at a specific position
    bool CheckForModul(Vector3 pos)
    {
        GameObject[] modules = GetAllModules();
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
        GameObject[] modules = GetAllModules();
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

    public static GameObject[] GetAllModules()
    {
        GameObject[] modules = GameObject.FindGameObjectsWithTag("Modul");
        GameObject[] corners = GameObject.FindGameObjectsWithTag("Corner");
        return modules.Concat(corners).ToArray();
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
