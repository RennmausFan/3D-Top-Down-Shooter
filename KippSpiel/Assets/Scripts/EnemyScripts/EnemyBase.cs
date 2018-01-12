using UnityEngine;

public class EnemyBase : MonoBehaviour{

    protected bool isFollowing;

    public int health = 100;
    public int reactRange = 10;
    public int cubeSpeed;

    protected GameObject player;

    [SerializeField]
    private GameObject deathParticle;

    [SerializeField]
    protected Rigidbody rb;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	protected void Update () {
	    if (health <= 0)
        {
            Die();
        }
	}

    public void moveCube(Vector3 pDirection)
    {
        rb.AddForce(pDirection * cubeSpeed * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            health -= other.gameObject.GetComponent<ShootBullet>().damage;
        }
    }

    //Triggered if health = 0
    protected void Die()
    {
        Instantiate(deathParticle, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void FollowPlayer()
    {
        Vector3 direction = player.transform.position - gameObject.transform.position;
        moveCube(direction);
        isFollowing = true;
    }

    //Returns true if player is in range
    protected bool CheckForDistance(float maxDistance)
    {
        /*
        RaycastHit hit;
        //If raycast hits anything else, than player --> false
        if (Physics.Raycast(gameObject.transform.position, player.transform.position, out hit))
        {
            if (hit.collider.tag != "Player")
            {
                print(hit.collider.tag);
                return false;
            }
        }
        */
        //Calculate distance (player <---> enemy)
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (distance > maxDistance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
