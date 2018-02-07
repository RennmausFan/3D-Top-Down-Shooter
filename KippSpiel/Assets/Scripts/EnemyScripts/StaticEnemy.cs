using UnityEngine;

public class StaticEnemy : EnemyBase {

    [SerializeField]
    private GameObject bullet;

    public float fireRateSec;
    public int bulletDamage;
    public int bulletSpeed;

    private float timerFireRate;

    void Update()
    {
        base.Update();
        if (!player.GetComponent<PlayerController>().isActiveAndEnabled)
        {
            return;
        }
        if (CheckForDistance(reactRange))
        {
            Vector3 lookAtVector = player.transform.position;
            lookAtVector.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(lookAtVector);
            if (timerFireRate >= fireRateSec)
            {
                //Only shoot at player if they are (nearly) on the same level
                float distanceY = gameObject.transform.position.y - player.transform.position.y;
                if (-0.1 <= distanceY && distanceY <= 0.1)
                {
                    ShootAtPlayer();
                    timerFireRate = 0f;
                }
            }
            timerFireRate += Time.deltaTime;
        }    
    }

    void ShootAtPlayer()
    {
        Vector3 targetPos = player.transform.position - gameObject.transform.position;
        GameObject obj = Instantiate(bullet, gameObject.transform.position, Quaternion.LookRotation(targetPos));
        obj.GetComponent<BulletController>().bulletDamage = bulletDamage;
        obj.GetComponent<BulletController>().bulletSpeed = bulletSpeed;

    }
}
