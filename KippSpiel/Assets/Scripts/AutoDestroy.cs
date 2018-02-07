using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    private ParticleSystem particle;


    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update () {
		if (!particle.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
