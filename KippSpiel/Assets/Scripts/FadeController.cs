using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour {

    Material mat;

    public float timeFadeIn, timeFadeOut;

    private bool fateIn, fateOut;

    public float fateInDestination, fateOutDestination;

	// Use this for initialization
	void Start () {
        mat = gameObject.GetComponent<Renderer>().material;
	}

    void Update()
    {
        Color newColor = mat.color;
        if (fateIn)
        {
            //FateIn
            newColor.a += Mathf.SmoothStep(0, 1, timeFadeIn * Time.deltaTime);
        }
        else if (fateOut && newColor.a >= 0.01f)
        {
            //FateOut
            newColor.a -= Mathf.SmoothStep(0, 1, timeFadeOut * Time.deltaTime);
        }

        if (newColor.a >= fateInDestination)
        {
            fateIn = false;
        }
        else if (newColor.a <= fateOutDestination)
        {
            fateOut = false;
        }

        mat.color = newColor;
    }

    public void FadeIn()
    {
        fateIn = true;
        fateOut = false;
    }

    public void FadeOut()
    {
        fateOut = true;
        fateIn = false;
    }
}
