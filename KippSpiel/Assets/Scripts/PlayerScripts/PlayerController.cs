using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour, I_VoxelColor
{

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject bullet;

    private static Vector3 spawnpoint = new Vector3(0, 2, 0);
    private static Vector3 deathpoint = new Vector3(0, 2, 0);

    public bool isOutside;

    public int health = 200;
    public static int glow;
    public static int stage = 1;

    public int cubeSpeed;
    public int dashPower;
    public int dashCount;
    public int dashMax;
    public float dashReloadSec;
    public float fireRateStart;
    public float fireRateMax;
    public float deathTime = 3f;

    [SerializeField]
    private float fireRateSec;

    private float timePassed_Dash;
    private float timePassed_Fire;

    private Vector3 shootDirection;

    [SerializeField]
    private Light cubeLight;

    [SerializeField]
    private GameObject deathParticle;

    private float timer_DoubleClick;
    private direction buttonClicked = direction.None;
    private float doubleClickTime = 0.5f;

    public Color voxelColor;

    void Update()
    {
        //Player dies if health equals 0
        if (health <= 0)
        {
            Die();
        }

        try
        {
            if (GameManager.GetRoom(gameObject).GetComponent<ModulManager>().isOutside)
            {
                isOutside = true;
            }
            else
            {
                isOutside = false;
            }
        }
        catch (NullReferenceException ex) { }

        //Perform movement
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        rb.AddForce(move * cubeSpeed * Time.deltaTime);

        //Shoot
        if (shootDirection != Vector3.zero)
        {
            if (timePassed_Fire > fireRateSec)
            {
                Shoot(shootDirection);
                timePassed_Fire = 0;
            }
        }
        timePassed_Fire += Time.deltaTime;

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

        //Timer for double click buttons
        if (buttonClicked != direction.None)
        {
            timer_DoubleClick += Time.deltaTime;
        }
        if (timer_DoubleClick > doubleClickTime)
        {
            timer_DoubleClick = 0f;
            buttonClicked = direction.None;
        }

        //Adjust Light
        cubeLight.intensity = Mathf.Lerp(cubeLight.intensity, dashCount, Time.deltaTime * 5);
    }

    #region Dash and Shoot

    void ShootLeft()
    {
        shootDirection = new Vector3(-1, 0, 0);
        if (buttonClicked == direction.Left)
        {
            Dash(Vector3.left);
        }
        buttonClicked = direction.Left;
    }

    void ShootRight()
    {
        shootDirection = new Vector3(1, 0, 0);
        if (buttonClicked == direction.Right)
        {
            Dash(Vector3.right);
        }
        buttonClicked = direction.Right;
    }

    void ShootUp()
    {
        shootDirection = new Vector3(0, 0, 1);
        if (buttonClicked == direction.Up)
        {
            Dash(new Vector3(0, 0, 1));
        }
        buttonClicked = direction.Up;
    }

    void ShootDown()
    {
        shootDirection = new Vector3(0, 0, -1);
        if (buttonClicked == direction.Down)
        {
            Dash(new Vector3(0, 0, -1));
        }
        buttonClicked = direction.Down;
    }

    void ShootLeftInactive()
    {
        shootDirection = Vector3.zero;
    }

    void ShootRightInactive()
    {
        shootDirection = Vector3.zero;
    }

    void ShootUpInactive()
    {
        shootDirection = Vector3.zero;
    }

    void ShootDownInactive()
    {
        shootDirection = Vector3.zero;
    }

    #endregion

    //Dash
    public void Dash(Vector3 direction)
    {
        if (dashCount <= 0)
        {
            return;
        }
        rb.AddForce(direction * dashPower);
        dashCount--;
        timePassed_Dash = 0;
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
        GameObject particle = Instantiate(deathParticle, gameObject.transform.position, Quaternion.identity);
        particle.GetComponent<Renderer>().material.color = voxelColor;
        gameObject.SetActive(false);
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
            TakeDamage(other.gameObject.GetComponent<BulletController>().bulletDamage);
        }
    }

    private bool CheckForGround(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position + direction, Vector3.down, out hit))
        {
            if (hit.collider.tag != "Player")
                return true;
        }
        return false;
    }

    #region "Get/Set"

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

    public int GetStage()
    {
        return stage;
    }

    public GameObject GetRoom()
    {
        return GameManager.GetRoom(gameObject);
    }

    public void SetStage(int pStage)
    {
        stage = pStage;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int pHealth)
    {
        health = pHealth;
    }

    public Color GetVoxelColor()
    {
        return voxelColor;
    }
    #endregion
}
