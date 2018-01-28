using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    private CubeActions _CubeActions;

    public int cubeSpeed = 50;
    public int dashPower = 1;
    public int dashCount = 5;
    public int dashMax = 5;
    public float dashReloadSec;
    public float fireRateStart;
    public float fireRateMax = 0.05f;

    [SerializeField]
    private float fireRateSec;
    
    private float timePassed_Dash;
    private float timePassed_Fire;

    [SerializeField]
    private Light cubeLight;

    void Start()
    {
        _CubeActions = gameObject.GetComponent<CubeActions>();    
    }

    // Update is called once per frame
    /*
    void Update()
    {
        print(Input.GetAxis("Horizontal"));
        print(Input.GetAxis("Vertical"));
        _CubeActions.MoveCube(new Vector3(CrossPlatformInputManager.GetAxis("Horizontal") * cubeSpeed, 0f, CrossPlatformInputManager.GetAxis("Vertical") * cubeSpeed));
        //Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1) || Input.GetButtonDown("Jump"))
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
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Fire1"))
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
        //Adjust fire rate by glow;
        fireRateSec = fireRateStart - GetComponent<CubeActions>().GetGlow() / 1000f;
        if (fireRateSec <= fireRateMax)
        {
            fireRateSec = 0.05f;
        }
    }
    */

    void Update()
    {
        _CubeActions.MoveCube(new Vector3(CrossPlatformInputManager.GetAxis("Horizontal") * cubeSpeed, 0f, CrossPlatformInputManager.GetAxis("Vertical") * cubeSpeed));

        float x = CrossPlatformInputManager.GetAxis("MouseX");
        float y = CrossPlatformInputManager.GetAxis("MouseY");
        //print(x);
        //print(y);
        Vector3 playerPos = gameObject.transform.position;
        Vector3 direction = new Vector3(playerPos.x + x, playerPos.y, playerPos.z + y);
        //print(direction);


        gameObject.transform.LookAt(direction);
        if (x != 0 || y != 0)
        {
            if (timePassed_Fire > fireRateSec)
            {
                _CubeActions.Shoot(new Vector3(x, 0f, y));
                timePassed_Fire = 0;
            }
            timePassed_Fire += Time.deltaTime;
        }

        //Adjust fire rate by glow;
        fireRateSec = fireRateStart - GetComponent<CubeActions>().GetGlow() / 1000f;
        if (fireRateSec <= fireRateMax)
        {
            fireRateSec = 0.05f;
        }
    }
}
