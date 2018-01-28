using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    [SerializeField]
    GameObject thisObj;

    [SerializeField]
    private GameObject hitParticle;

    [SerializeField]
    private string[] tagToIgnore;

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
        if (!ArrayContains<string>(tagToIgnore, other.tag))
        {
            Instantiate(hitParticle, thisObj.transform.position, Quaternion.identity);
            Destroy(thisObj);
        }
    }

    public static bool ArrayContains <T>(T[] array, T obj)
    {
        foreach(T arrayObj in array)
        {
            if (arrayObj.Equals(obj))
            {
                return true;
            }
        }
        return false;
    }
}
