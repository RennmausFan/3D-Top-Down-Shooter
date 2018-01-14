using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    [SerializeField]
    ParticleSystem particle;

	// Update is called once per frame
	void Update () {
		if (!particle.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
