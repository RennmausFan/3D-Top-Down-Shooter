using UnityEngine;

public class EnemyBase : MonoBehaviour, I_VoxelColor{

    protected bool isFollowing;

    public int health = 100;
    public float reactRange = 10;
    public int speed;

    public int glowDrop;

    protected GameObject parentRoom;
    protected GameObject player;

    [SerializeField]
    protected GameObject deathParticle;

    [SerializeField]
    protected Rigidbody rb;

    public Color voxelColor;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        parentRoom = GameManager.GetRoom(gameObject);
    }
	
	// Update is called once per frame
	protected void Update () {
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
    protected void Die()
    {
        GameObject particle = Instantiate(deathParticle, gameObject.transform.position, Quaternion.identity);
        particle.GetComponent<Renderer>().material.color = voxelColor;
        player.SendMessage("AddGlow", glowDrop);
        Destroy(gameObject);
    }

    //Make the enemy follow the player
    protected void FollowPlayer()
    {
        Vector3 direction = player.transform.position - gameObject.transform.position;
        rb.AddForce(direction * speed * Time.deltaTime);
        isFollowing = true;
    }

    //Returns true if player is in range
    protected bool CheckForDistance(float maxDistance)
    {
        //Calculate distance (player <---> enemy)
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance > maxDistance)
        {
            return false;
        }
        else
        {
            if (GameManager.GetRoom(player) == parentRoom || player.GetComponent<PlayerController>().isOutside)
            {
                return true;
            }
            
        }
        return false;
    }

    public Color GetVoxelColor()
    {
        return voxelColor;
    }
}
