using UnityEngine;

public class StaticEnemy : EnemyBase {

    [SerializeField]
    private GameObject bullet;

    public float fireRateSec;

    private float timerFireRate;

    void Update()
    {
        base.Update();
        if (CheckForDistance(reactRange))
        {
            Vector3 lookAtVector = player.transform.position;
            lookAtVector.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(lookAtVector/* * Time.deltaTime * cubeSpeed*/);
            if (timerFireRate >= fireRateSec)
            {
                ShootAtPlayer();
                timerFireRate = 0f;
            }
            timerFireRate += Time.deltaTime;
        }    
    }

    void ShootAtPlayer()
    {
        Vector3 targetPos = player.transform.position - gameObject.transform.position;
        Instantiate(bullet, gameObject.transform.position, Quaternion.LookRotation(targetPos));
    }
}
