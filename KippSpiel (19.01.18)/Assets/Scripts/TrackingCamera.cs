using UnityEngine;

public class TrackingCamera : MonoBehaviour {

    [SerializeField]
    private GameObject trackingObj;

    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void Start()
    {
        trackingObj = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate () {
        //Calculate newPos and then smoothly translate between the current an the new position
        Vector3 newPos = trackingObj.transform.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPos;
	 }
}
