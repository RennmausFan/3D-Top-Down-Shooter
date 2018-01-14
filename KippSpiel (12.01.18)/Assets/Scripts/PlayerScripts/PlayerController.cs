using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CubeActions _CubeActions;

    public int cubeSpeed = 50;
    public int dashPower = 1;
    public int dashCount = 5;
    public int dashMax = 5;
    public float dashReloadSec;
    public float fireRateSec;
    
    private float timePassed_Dash;
    private float timePassed_Fire;

    [SerializeField]
    private Light cubeLight;

    void Start()
    {
        _CubeActions = gameObject.GetComponent<CubeActions>();    
    }

    // Update is called once per frame
    void Update()
    {
        //Tip Forward
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _CubeActions.MoveCube(new Vector3(0, 0, cubeSpeed));

        }
        //Tip Backwards
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _CubeActions.MoveCube(new Vector3(0, 0, -cubeSpeed));

        }
        //Tip Left
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _CubeActions.MoveCube(new Vector3(-cubeSpeed, 0, 0));

        }
        //Tip Right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _CubeActions.MoveCube(new Vector3(cubeSpeed, 0, 0));

        }
        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1))
        {
            if (dashCount > 0)
            {
                Vector3 direction = _CubeActions.GetMouseDirection().normalized;
                if (direction == Vector3.zero)
                {
                    return;
                }
                _CubeActions.Dash(direction * dashPower);
                dashCount--;
                timePassed_Dash = 0;
            }
        }
        //Rapid-Fire
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            if (timePassed_Fire > fireRateSec)
            {
                _CubeActions.Shoot();
                timePassed_Fire = 0;
            }
            timePassed_Fire += Time.deltaTime;
        }
        //Reload Dash
        timePassed_Dash += Time.deltaTime;
        if (timePassed_Dash > dashReloadSec && dashCount < dashMax)
        {
            dashCount++;
            timePassed_Dash = 0;
        }
        //Adjust Light
        cubeLight.intensity = Mathf.Lerp(cubeLight.intensity, dashCount, Time.deltaTime *5);
    }
}
