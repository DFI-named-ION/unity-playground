using UnityEngine;
using Assets.Scripts.VR.Core;

public class ControllerMovement : MonoBehaviour
{
    public float Speed = 5f;
    public Transform DirectionTransform;
    public Transform PlayerTransform;
    public LeftControllerEvents ControllerEvents;

    private void Awake()
    {
        ControllerEvents.OnStickAxisChange += OnStickAxisChange;
    }

    private void OnStickAxisChange(Vector2 axis)
    {
        Vector3 forward = DirectionTransform.forward;
        Vector3 right = DirectionTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * axis.y + right * axis.x;
        PlayerTransform.position += moveDir * Speed * Time.deltaTime;
    }
}