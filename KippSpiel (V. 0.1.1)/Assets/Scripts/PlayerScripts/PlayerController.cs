using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject bullet;

    private static Vector3 spawnpoint = new Vector3(0, 2, 0);
    private static Vector3 deathpoint = new Vector3(0, 2, 0);

    public int health = 100;

    public static int glow;
    public static int stage = 1;


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


    void Update()
    {
        //Player dies if health equals 0
        if (health <= 0)
        {
            Die();
        }

        //Perform movement
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        rb.AddForce(move.normalized * cubeSpeed * Time.deltaTime);

        //print(h + " / " + v);

        //Calculate direction
        float x = CrossPlatformInputManager.GetAxis("MouseX");
        float y = CrossPlatformInputManager.GetAxis("MouseY");
        Vector3 playerPos = gameObject.transform.position;
        Vector3 direction = new Vector3(playerPos.x + x, playerPos.y, playerPos.z + y);

        //print(x + " / " + y);

        //Look at direction
        gameObject.transform.LookAt(direction);

        //Shoot
        if (x != 0 || y != 0)
        {
            if (timePassed_Fire > fireRateSec)
            {
                Shoot(new Vector3(x, 0f, y));
                timePassed_Fire = 0;
            }
            timePassed_Fire += Time.deltaTime;
        }

        //Adjust fire rate by glow;
        fireRateSec = fireRateStart - GetComponent<PlayerController>().GetGlow() / 1000f;
        if (fireRateSec <= fireRateMax)
        {
            fireRateSec = 0.05f;
        }

        //Reload dash
        if (timePassed_Dash >= dashReloadSec && dashCount < dashMax)
        {
            dashCount++;
            timePassed_Dash = 0f;
        }
        timePassed_Dash += Time.deltaTime;

        //Adjust Light
        cubeLight.intensity = Mathf.Lerp(cubeLight.intensity, dashCount, Time.deltaTime * 5);
    }

    void DashLeft()
    {
        if (dashCount > 0)
        {
            rb.AddForce(Vector3.left * dashPower);
            dashCount--;
            timePassed_Dash = 0;
        }
    }

    void DashRight()
    {
        if (dashCount > 0)
        {
            rb.AddForce(Vector3.right * dashPower);
            dashCount--;
            timePassed_Dash = 0;
        }
    }

    //Shoot by creating an instance of a bullet facing mouse direction
    public void Shoot(Vector3 direction)
    {
        Vector3 playerPos = gameObject.transform.position;
        playerPos.y += 0.5f;
        Instantiate(bullet, playerPos, Quaternion.LookRotation(direction));       
    }

    //Get the vector between the mouse cursor and the player (screen space based)
    public Vector3 GetMouseDirection()
    {
        Vector3 playerPos = Camera.main.WorldToScreenPoint(rb.transform.position);
        playerPos.z = 0;
        Vector3 relativePos = Input.mousePosition - playerPos;
        relativePos.z = relativePos.y;
        relativePos.y = rb.transform.position.y;

        return relativePos;
    }

    //Teleport Player back to spawn
    public void TeleportToSpawnpoint()
    {
        gameObject.transform.position = spawnpoint;
    }

    //Player dies
    public void Die()
    {
        deathpoint = gameObject.transform.position;
        print("Player died! How cruel! ;-;");
        GameObject.FindWithTag("GameManager").SendMessage("RestartGame");

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            TakeDamage(other.gameObject.GetComponent<BulletController>().damage);
        }
    }

    #region "Get/Set"

    public void SetSpawnpoint(Vector3 pSpawnpoint)
    {
        spawnpoint = pSpawnpoint;
    }

    public Vector3 GetSpawnpoint()
    {
        return spawnpoint;
    }

    public void SetDeathpoint(Vector3 pDeathpoint)
    {
        deathpoint = pDeathpoint;
    }

    public Vector3 GetDeathpoint()
    {
        return deathpoint;
    }

    public void SetGlow(int pGlow)
    {
        glow = pGlow;
    }

    public int GetGlow()
    {
        return glow;
    }

    public void AddGlow(int pGlow)
    {
        glow += pGlow;
    }

    public GameObject GetRoom()
    {
        //Find room player currently in
        GameObject[] modules = GameObject.FindGameObjectsWithTag("Modul").Concat(GameObject.FindGameObjectsWithTag("Corner")).ToArray();

        foreach (GameObject modul in modules)
        {
            Vector3 modulPos = modul.transform.position;
            Vector3 playerPos = gameObject.transform.position;

            if (playerPos.x < (modulPos.x + 15) && playerPos.x > (modulPos.x - 15) && playerPos.z < (modulPos.z + 15) && playerPos.z > (modulPos.z - 15))
            {
                return modul;
            }
        }
        return null;
    }

    public int GetStage()
    {
        return stage;
    }

    public void SetStage(int pStage)
    {
        stage = pStage;
    }
    #endregion

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
}
