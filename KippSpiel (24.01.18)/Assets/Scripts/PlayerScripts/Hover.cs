using UnityEngine;

public class Hover : MonoBehaviour {

    public float hoverHeight;
    public float upSpeed, downSpeed;

    [SerializeField]
    private Rigidbody rb;

	// Update is called once per frame
	void Update () {
        float d = CheckForDistance();
        if (d > hoverHeight)
        {
            //Move downwards
            rb.transform.position += new Vector3(0, -(d - hoverHeight) * Time.deltaTime * downSpeed, 0);
        }
        else if (d < hoverHeight)
        {
            //Move upwards
            rb.transform.position += new Vector3(0, (hoverHeight - d) * Time.deltaTime * upSpeed, 0);
        }
	}

    //Calculate distance between player and ground
    private float CheckForDistance()
    {
        float distance = hoverHeight + 1;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0,-1,0), out hit))
        {
            distance = Vector3.Magnitude(hit.point - transform.position);
            //print(distance);       
        }
        return distance;
    }
}
