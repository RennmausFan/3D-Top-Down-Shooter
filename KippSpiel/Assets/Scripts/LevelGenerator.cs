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
    [SerializeField]
    private GameObject goalModul;
    [SerializeField]
    private GameObject transitionModul;
    [SerializeField]
    private GameObject startModul_Outside;
    [SerializeField]
    private GameObject goalModul_Outside;

    [SerializeField]
    private int countAll;
    [SerializeField]
    private int countInside;
    [SerializeField]
    private int countOutside;

    public int maxDepth;
    [Range(0,1)]
    public float modulRate;
    public int minCountInside;
    public int minCountOutside;

    [SerializeField]
    private bool startOutside = true;

    // Use this for initialization
    void Awake()
    {
        //Instantiate start modul
        GameObject start = Instantiate(startModul_Outside, Vector3.zero, RandomRotation());
        countOutside++;

        //First modul
        Vector3 pos = start.GetComponent<ModulManager>().RandomPosition();
        GameObject newModul = Instantiate(RandomModul(outsideModules), pos, RandomRotation());
        countOutside++;

        //Recursiv constructing of the level
        BuildOutside(newModul, 0);

        //Activate invisible walls
        CalculateBorders();

        //Find modul furthest away and replace it with an transition modul
        GameObject temp = GetModulFurthestAway(start, true);
        GameObject transModul = CalculateTransition(temp);
        countInside++;

        //Generate the inside moduls
        BuildInside(transModul, 0, direction.None);

        //FixLevel

        //Find modul furthest away and replace it with an goal modul
        GameObject temp2 = GetModulFurthestAway(transModul, false);
        GameObject goal = ReplaceModul(temp2, goalModul);

        countAll = GetAllModules().Length;
    }

    #region BuildInside

    void BuildInside(GameObject modul, int depth, direction ignoreDir)
    {
        GameObject[] useModules = modules;

        if (depth >= 3 && countInside > minCountInside) useModules = cornerModules;

        ModulManager script = modul.GetComponent<ModulManager>();
        direction[] doors = script.GetDoors().ToArray();
        GameObject left = null, right = null, up = null, down = null;

        if (doors.Contains(direction.Left) && ignoreDir != direction.Left)
        {
            left = CreateModulInside(useModules, script.posLeft, direction.Right, modulRate);
        }
        if (doors.Contains(direction.Right) && ignoreDir != direction.Right)
        {
            right = CreateModulInside(useModules, script.posRight, direction.Left, modulRate);
        }
        if (doors.Contains(direction.Up) && ignoreDir != direction.Up)
        {
            up = CreateModulInside(useModules, script.posUp, direction.Down, modulRate);
        }
        if (doors.Contains(direction.Down) && ignoreDir != direction.Down)
        {
            down = CreateModulInside(useModules, script.posDown, direction.Up, modulRate);
        }

        if (left != null) BuildInside(left, depth + 1, direction.Right);
        if (right != null) BuildInside(right, depth + 1, direction.Left);
        if (up != null) BuildInside(up, depth + 1, direction.Down);
        if (down != null) BuildInside(down, depth + 1, direction.Up);

    }

    GameObject CreateModulInside(GameObject[] pModules, Vector3 pos, direction faceDir, float chance)
    {
        //Return if there is already a modul
        if (CheckForModul(pos)) return null;

        GameObject newModul;

        //Create at least "minModulCount" modules, then create them by chance
        if (countInside > minCountInside)
        {
            float random = Random.Range(0f, 1f);
            if (random >= chance)
            {
                newModul = Instantiate(RandomModul(cornerModules), pos, Quaternion.identity);
            }
            else newModul = Instantiate(RandomModul(pModules), pos, Quaternion.identity);
        }
        else
        {
            newModul = Instantiate(RandomModul(pModules), pos, Quaternion.identity);
        }
        newModul.GetComponent<ModulManager>().RotateToDoor(faceDir);
        countInside++;
        return newModul;
    }

    GameObject CalculateTransition(GameObject modul)
    {
        ModulManager script = modul.GetComponent<ModulManager>();
        Vector3 pos = Vector3.zero;
        direction rotateTo = direction.None;
        if (!CheckForModul(script.posLeft))
        {
            pos = script.posLeft;
            rotateTo = direction.Right;
            script.borderLeft.SetActive(false);
        }
        else if (!CheckForModul(script.posRight))
        {
            pos = script.posRight;
            rotateTo = direction.Left;
            script.borderRight.SetActive(false);
        }
        else if (!CheckForModul(script.posUp))
        {
            pos = script.posUp;
            rotateTo = direction.Down;
            script.borderUp.SetActive(false);
        }
        else if (!CheckForModul(script.posDown))
        {
            pos = script.posDown;
            rotateTo = direction.Up;
            script.borderDown.SetActive(false);
        }

        if (pos == Vector3.zero) print("ERROR: At CalculateTransition()");
        GameObject trans = Instantiate(transitionModul, pos, Quaternion.identity);
        trans.GetComponent<ModulManager>().RotateToDoor(rotateTo);
        return trans;
    }
    #endregion

    #region BuildOutside

    void BuildOutside(GameObject modul, int depth)
    {
        if (depth >= 3 && countOutside > minCountOutside) return;

        ModulManager script = modul.GetComponent<ModulManager>();

        GameObject left = CreateModulOutside(script.posLeft, modulRate);
        GameObject right = CreateModulOutside(script.posRight, modulRate);
        GameObject up = CreateModulOutside(script.posUp, modulRate);
        GameObject down = CreateModulOutside(script.posDown, modulRate);

        if (left != null) BuildOutside(left, depth + 1);
        if (right != null) BuildOutside(right, depth + 1);
        if (up != null) BuildOutside(up, depth + 1);
        if (down != null) BuildOutside(down, depth + 1);
    }

    GameObject CreateModulOutside(Vector3 pos, float chance)
    {
        //Return if there is already a modul
        if (CheckForModul(pos)) return null;

        //Create at least "minModulCount" modules, then create them by chance
        if (countOutside > minCountOutside)
        {
            float random = Random.Range(0f, 1f);
            if (random >= chance) return null;
        }

        GameObject newModul = Instantiate(RandomModul(outsideModules), pos, RandomRotation());
        countOutside++;
        return newModul;
    }
    #endregion

    GameObject ReplaceModul(GameObject modul, GameObject newModul)
    {
        Transform location = modul.transform;
        GameObject temp = Instantiate(newModul, location.position, location.rotation);
        Destroy(modul);
        return temp;
    }

    #region CalculateBorders

    void CalculateBorders()
    {
        GameObject[] outsideModules = GameObject.FindGameObjectsWithTag("OutsideModul");
        foreach (GameObject modul in outsideModules)
        {
            EnableBordersForModul(modul);
        }
    }

    void EnableBordersForModul(GameObject modul)
    {
        ModulManager script = modul.GetComponent<ModulManager>();
        if (!CheckForModul(script.posLeft)) script.borderLeft.SetActive(true);
        if (!CheckForModul(script.posRight)) script.borderRight.SetActive(true);
        if (!CheckForModul(script.posUp)) script.borderUp.SetActive(true);
        if (!CheckForModul(script.posDown)) script.borderDown.SetActive(true);
    }

    #endregion

    #region FixLevel

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

    #endregion

    #region Util

    //Returns the modul furthest away
    GameObject GetModulFurthestAway(GameObject target, bool isOutside)
    {
        GameObject distanceModul = null;
        float maxDistance = 0;

        GameObject[] modules = GetAllModules();
        Vector3 targetPos = target.transform.position;

        foreach (GameObject modul in modules)
        {
            float distance = Vector3.Distance(targetPos, modul.transform.position);
            if (distance > maxDistance && modul.GetComponent<ModulManager>().isOutside == isOutside)
            {
                maxDistance = distance;
                distanceModul = modul;
            }
        }
        return distanceModul;
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

    //Returns all Modules as an array
    public static GameObject[] GetAllModules()
    {
        GameObject[] modules = GameObject.FindGameObjectsWithTag("Modul");
        GameObject[] corners = GameObject.FindGameObjectsWithTag("Corner");
        GameObject[] outsideModules = GameObject.FindGameObjectsWithTag("OutsideModul");
        return modules.Concat(corners).Concat(outsideModules).ToArray();
    }

    //Returns random modul from selected array
    GameObject RandomModul(GameObject[] modules)
    {
        int random = (int)Random.Range(0f, modules.Length);
        return modules[random];
    }

    //Returns either a rotation of 0, 90, 180 or -90 as Quarternion 
    Quaternion RandomRotation()
    {
        float random = Random.Range(0f, 1f);
        float rotation = 0f;

        if (random <= 0.25f)
        {
            rotation = 0f;
        }
        else if (random > 0.25f && random <= 0.5f)
        {
            rotation = 90f;
        }
        else if (random > 0.5f && random <= 0.75f)
        {
            rotation = 180f;
        }
        else
        {
            rotation = -90f;
        }
        Vector3 rotationVec = new Vector3(0f, rotation, 0f);
        return Quaternion.Euler(rotationVec);
    }
    #endregion
}
