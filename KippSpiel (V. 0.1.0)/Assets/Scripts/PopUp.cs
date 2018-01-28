using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {

    private GameObject player;

    public float duration;
    public float timer;

    public bool displayStage;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (displayStage)
        {
            GetComponent<Text>().text = "Stage: " + player.GetComponent<CubeActions>().GetStage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > duration)
        {
            Destroy(gameObject);
        }
        Color newColor = GetComponent<Text>().color;
        float ratio = timer / duration;
        newColor.a = Mathf.Lerp(1, 0, ratio);
        GetComponent<Text>().color = newColor;
        timer += Time.deltaTime;
    }
}
