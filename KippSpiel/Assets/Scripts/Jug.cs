using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jug : MonoBehaviour, I_VoxelColor {

    public float health = 100;

    [SerializeField]
    private GameObject deathParticle;

    public int glowDrop;

    public Color voxelColor;

    private GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    //Enemy can take damage by shooting at them
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            health -= other.gameObject.GetComponent<BulletController>().bulletDamage;
        }
    }

    //Triggered if health = 0
    void Die()
    {
        GameObject particle = Instantiate(deathParticle, gameObject.transform.position, Quaternion.identity);
        particle.GetComponent<Renderer>().material.color = voxelColor;
        player.SendMessage("AddGlow", glowDrop);
        Destroy(gameObject);
    }

    public Color GetVoxelColor()
    {
        return voxelColor;
    }
}
