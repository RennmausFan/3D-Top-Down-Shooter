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
        //print(targetPos);
        Instantiate(bullet, gameObject.transform.position, Quaternion.LookRotation(targetPos));
    }
}
