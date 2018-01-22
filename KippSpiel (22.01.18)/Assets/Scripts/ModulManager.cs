using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Global enum
public enum direction { Left, Right, Up, Down };
public enum state { Empty, Wall, Door };

public class ModulManager : MonoBehaviour {
    
    //Defined manual for each modul
    [SerializeField]
    private state baseLeft, baseRight, baseUp, baseDown;

    //Calculated based on the moduls rotation
    [SerializeField]
    private state left, right, up, down;

	// Use this for initialization
	void Awake ()
    {
        CalculateStates();
	}

    //Switch states based on rotation
    public void CalculateStates()
    {
        left = baseLeft;
        right = baseRight;
        up = baseUp;
        down = baseDown;

        int rotation = (int)gameObject.transform.rotation.eulerAngles.y;
        switch (rotation)
        {
            case 0:
                //Nothing
                break;
            case 90:
                SwapStates(1);
                break;
            case 180:
                SwapStates(2);
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
    }

    //Returns a List with the direction of the doors this modul has
    public LinkedList<direction> GetDoors()
    {
        LinkedList<direction> doors = new LinkedList<direction>();
        if (left == state.Door)
        {
            print("Modul has door left");
            doors.AddLast(direction.Left);
        }
        if (right == state.Door)
        {
            print("Modul has door right");
            doors.AddLast(direction.Right);
        }
        if (up == state.Door)
        {
            print("Modul has door up");
            doors.AddLast(direction.Up);
        }
        if (down == state.Door)
        {
            print("Modul has door down");
            doors.AddLast(direction.Down);
        }
        return doors;
    }

    //Rotate modul so that one door faces the given direction
    public void RotateToDoor(state doorNeeded)
    {

        //while (doorNeeded != state.Door)
        {
            gameObject.transform.Rotate(0, 90, 0);
            CalculateStates();
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
    #endregion
}
