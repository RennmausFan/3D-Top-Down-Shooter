using UnityEngine;

public class ExplosionEnemy : EnemyBase{

    bool isCharging;

    public float chargingRange = 5f;
    public float chargeTime = 1f;

    public float explosionForce = 800f;
    public float explosionRadius = 10f;
    public int explosionDamage = 100;

    float chargeTimer;

    void Update()
    {
        base.Update();

        if (CheckForDistance(reactRange))
        {
            FollowPlayer();
            isFollowing = true;
        }
        if (CheckForDistance(chargingRange) && isFollowing)
        {
            isCharging = true;
        }
        else
        {
            isCharging = false; chargeTimer = 0f;
        }
        chargeTimer += Time.deltaTime;
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
