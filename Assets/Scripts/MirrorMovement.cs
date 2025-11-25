using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    public Transform Player;
    public Transform Mirror;

    void Update()
    {
        Vector3 local = Mirror.InverseTransformPoint(Player.position);
        transform.position = Mirror.TransformPoint(new Vector3(local.x, local.y, -local.z));

        Vector3 lookAtMirror = Mirror.TransformPoint(new Vector3(-local.x, local.y, local.z));
        transform.LookAt(lookAtMirror);
    }
}