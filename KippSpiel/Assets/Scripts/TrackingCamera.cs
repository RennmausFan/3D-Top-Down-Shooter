using UnityEngine;

public class TrackingCamera : MonoBehaviour {

    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public float speedInside = 0.125f;
    public float speedOutside= 0.125f;

    private PlayerController player;
    private Camera cam;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        cam = GetComponent<Camera>();
    }

    void LateUpdate ()
    {
        if (player.isOutside)
        {
            //Free camera movement and specific camera settings if player is ouside
            TrackObject(player.gameObject, speedOutside);
            cam.clearFlags = CameraClearFlags.Skybox;
            cam.cullingMask |= (1 << LayerMask.NameToLayer("OutsideModul"));

        }
        else
        {
            //Fixed camera movement and specific camera settings if player is inside
            TrackObject(GameManager.GetRoom(player.gameObject), speedInside);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer("OutsideModul"));
        }
	}

    //Smoothly translates to a position of the given object (used in Update())
    void TrackObject(GameObject target, float speed)
    {
        Vector3 newPos = target.transform.position + offsetPos;
        //gameObject.transform.rotation = offsetRot;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, Time.deltaTime * speed);
        transform.position = smoothedPos;
    }
}
