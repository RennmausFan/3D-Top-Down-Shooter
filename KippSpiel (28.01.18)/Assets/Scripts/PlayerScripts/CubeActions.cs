using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeActions : MonoBehaviour
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

    //public float lookRotationMultiplier = 10f;

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        //Look at mouse
        //gameObject.transform.LookAt(GetMouseDirection());// * Time.deltaTime);
    }

    //Perform a roll
    public void MoveCube(Vector3 pDirection)
    {
        //transform.Translate(pDirection * Time.deltaTime, Space.World);
        rb.AddForce(pDirection * Time.deltaTime);
    }

    //Perform a dash
    public void Dash(Vector3 pDirection)
    {
        //transform.Translate(pDirection, Space.World);
        rb.AddForce(pDirection);
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
}
