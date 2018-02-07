using UnityEngine;

public class ExplosionEnemy : EnemyBase{

    public float chargingRange;
    public float chargeTime;

    public float explosionForce;
    public float explosionRadius;
    public int explosionDamage;

    float chargeTimer;

    public float scaleMultiplier = 2f;
    private Vector3 initialSize = new Vector3(1, 1, 1);


    void Update()
    {
        base.Update();
        //Stops shooting if player is dead
        if (!player.GetComponent<PlayerController>().isActiveAndEnabled)
        {
            return;
        }

        //If in react range --> move towards player
        if (CheckForDistance(reactRange))
        {
            FollowPlayer();
        }

        //If enemy gets close enough --> activate timer 
        if (!CheckForDistance(chargingRange))
        {
            chargeTimer = 0f;
        }

        //Resize based on chargeTimer
        Vector3 maxScale = initialSize * scaleMultiplier;
        float percentage = 1 / (chargeTime / chargeTimer);
        Vector3 newScale = Vector3.Lerp(initialSize, maxScale, percentage);
        gameObject.transform.localScale = newScale;

        chargeTimer += Time.deltaTime;

        //If timer is over --> EXPLODE
        if (chargeTimer >= chargeTime)
        {
            Explode();
        }

    }

    void Explode()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider obj in colliders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null && rb != gameObject.GetComponent<Rigidbody>())
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                if (rb.tag == "Player")
                {
                    player.SendMessage("TakeDamage", explosionDamage);
                }
            }
        }
        Die();
    }

}
