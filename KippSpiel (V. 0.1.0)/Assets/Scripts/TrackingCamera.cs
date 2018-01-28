using UnityEngine;

public class TrackingCamera : MonoBehaviour {

    private GameObject player;

    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        GameObject playerRoom = player.GetComponent<CubeActions>().GetRoom();
        if (!playerRoom.GetComponent<ModulManager>().GetIsOutside())
        {
            TrackObject(playerRoom);
        }
        else
        {
            TrackObject(player);
        }
	}

    void TrackObject(GameObject target)
    {
        Vector3 newPos = target.transform.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPos;
    }
}
