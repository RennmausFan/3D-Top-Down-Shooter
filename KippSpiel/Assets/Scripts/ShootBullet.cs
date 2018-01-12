using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour {

    [SerializeField]
    GameObject thisObj;

    [SerializeField]
    private GameObject hitParticle;

    public float speed;
    public float lifeTime = 12f;
    public int damage = 25;

    private float timePassed;

	// Update is called once per frame
	void Update () {
        MoveBullet();
        if (timePassed > lifeTime)
        {
            Destroy(thisObj);
        }
        timePassed += Time.deltaTime;

	}

    private void MoveBullet()
    {
        thisObj.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Bullet" && other.gameObject.tag != "IgnoreBullets")
        {
            Instantiate(hitParticle, thisObj.transform.position, Quaternion.identity);
            Destroy(thisObj);
        }
    }

}
