using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeActions : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject bullet;

    private static Vector3 spawnpoint = new Vector3(0, 2, 0);

    public int health = 100;

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
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
    public void Shoot()
    {
        Vector3 playerPos = rb.transform.position;
        Instantiate(bullet, playerPos, Quaternion.LookRotation(GetMouseDirection()));       
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
            TakeDamage(other.gameObject.GetComponent<ShootBullet>().damage);
        }
    }

    public void SetSpawnpoint(Vector3 pSpawnpoint)
    {
        spawnpoint = pSpawnpoint;
    }

    public Vector3 GetSpawnpoint()
    {
        return spawnpoint;
    }
}
