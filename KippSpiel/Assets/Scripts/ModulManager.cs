using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Global enum
public enum direction { Left, Right, Up, Down, None};
public enum state { Empty, Wall, Door };

public class ModulManager : MonoBehaviour {
    
    //Defined manual for each modul
    [SerializeField]
    private state baseLeft, baseRight, baseUp, baseDown;

    //Calculated based on the moduls rotation
    [SerializeField]
    private state left, right, up, down;

    public bool isOutside;

    public int doorCount;

    public Vector3 posLeft;
    public Vector3 posRight;
    public Vector3 posUp;
    public Vector3 posDown;

    private PlayerController player;
    private bool fadedOut;

    [SerializeField]
    private GameObject borderLeftBase, borderRightBase, borderUpBase, borderDownBase;

    public GameObject borderLeft, borderRight, borderUp, borderDown;

    // Use this for initialization
    void Awake()
    {
        CalculateStates();

        posLeft = gameObject.transform.position + new Vector3(-30, 0, 0);
        posRight = gameObject.transform.position + new Vector3(30, 0, 0);
        posUp = gameObject.transform.position + new Vector3(0, 0, 30);
        posDown = gameObject.transform.position + new Vector3(0, 0, -30);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isOutside)
        {
            return;
        }

        if (GameManager.GetRoom(player.gameObject) == gameObject && !fadedOut)
        {
            gameObject.GetComponentInChildren<FadeController>().FadeOut();
            fadedOut = true;
        }
        else if (GameManager.GetRoom(player.gameObject) != gameObject && fadedOut)
        {
            gameObject.GetComponentInChildren<FadeController>().FadeIn();
            fadedOut = false;
        }
    }

    //Switch states based on rotation
    public void CalculateStates()
    {
        //Set to initial value
        left = baseLeft;
        right = baseRight;
        up = baseUp;
        down = baseDown;

        if (isOutside)
        {
            borderLeft = borderLeftBase;
            borderRight = borderRightBase;
            borderUp = borderUpBase;
            borderDown = borderDownBase;
        }

        int rotation = (int)gameObject.transform.rotation.eulerAngles.y;
        switch (rotation)
        {
            case 0:
                //Nothing
                break;
            case 90:
                SwapStates(1);
                break;
            case -270:
                SwapStates(1);
                break;
            case 180:
                SwapStates(2);
                break;
            case -180:
                SwapStates(2);
                break;
            case 270:
                SwapStates(3);
                break;
            case -90:
                SwapStates(3);
                break;
        }
    }

    //Swaps the states clockwise
    void SwapStates(int swap)
    {
        for (int i=0; i<swap; i++)
        {
            state temp = up; 
            up = left;
            left = down;
            down = right;
            right = temp;
        }

        if (!isOutside) return;

        for (int z = 0; z < swap; z++)
        {
            GameObject temp = borderUp;
            borderUp = borderLeft;
            borderLeft = borderDown;
            borderDown = borderRight;
            borderRight = temp;
        }
    }

    //Returns a List with the direction of the doors this modul has
    public LinkedList<direction> GetDoors()
    {
        LinkedList<direction> doors = new LinkedList<direction>();
        if (left == state.Door)
        {
            doors.AddLast(direction.Left);
        }
        if (right == state.Door)
        {
            doors.AddLast(direction.Right);
        }
        if (up == state.Door)
        {
            doors.AddLast(direction.Up);
        }
        if (down == state.Door)
        {
            doors.AddLast(direction.Down);
        }
        return doors;
    }

    //Rotate modul so that one door faces the given direction
    public void RotateToDoor(direction doorNeeded)
    {
        if (doorNeeded == direction.Left)
        {
            while (left != state.Door)
            {
                gameObject.transform.Rotate(0, 90, 0);
                CalculateStates();
            }
        }
        else if (doorNeeded == direction.Right)
        {
            while (right != state.Door)
            {
                gameObject.transform.Rotate(0, 90, 0);
                CalculateStates();
            }
        }
        else if (doorNeeded == direction.Up)
        {
            while (up != state.Door)
            {
                gameObject.transform.Rotate(0, 90, 0);
                CalculateStates();
            }
        }
        else if (doorNeeded == direction.Down)
        {
            while (down != state.Door)
            {
                gameObject.transform.Rotate(0, 90, 0);
                CalculateStates();
            }
        }
    }

    //Returns either posLeft, posRight, posUp or posDown by random
    public Vector3 RandomPosition()
    {
        float random = Random.Range(0f, 1f);

        if (random <= 0.25f)
        {
            return posLeft;
        }
        else if (random > 0.25f && random <= 0.5f)
        {
            return posRight;
        }
        else if (random > 0.5f && random <= 0.75f)
        {
            return posUp;
        }
        else
        {
            return posDown;
        }
    }

    #region Get Methods
    public state GetLeft()
    {
        return left;
    }
    public state GetRight()
    {
        return right;
    }
    public state GetUp()
    {
        return up;
    }
    public state GetDown()
    {
        return down;
    }
    public bool GetIsOutside()
    {
        return isOutside;
    }
    #endregion
}
