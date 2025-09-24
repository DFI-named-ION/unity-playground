using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform Target = null;

    public float StartDistance = 2.5f;
    public float MinDistance = 1.5f;
    public float MaxDistance = 5f;

    public float HorizontalSensitivity = 0.25f;
    public float VerticalSensitivity = 0.25f;

    public float ZoomSpeed = 1f;

    private float _yaw;
    private float _pitch;
    private float _distance;

    void Start()
    {
        _distance = Mathf.Clamp(StartDistance, MinDistance, MaxDistance);
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
    }

    void LateUpdate()
    {
        if (Target == null) return;

        Vector2 mouseDelta = Vector2.zero;
        float scroll = 0f;

        if (Mouse.current != null)
        {
            mouseDelta = Mouse.current.delta.ReadValue();
            scroll = Mouse.current.scroll.ReadValue().y;
        }

        if (mouseDelta.sqrMagnitude > 0.0001f)
        {
            _yaw += mouseDelta.x * HorizontalSensitivity;
            _pitch -= mouseDelta.y * VerticalSensitivity;
            _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        }

        if (Mathf.Abs(scroll) > 0.0001f)
        {
            _distance -= scroll * ZoomSpeed * Time.deltaTime * 60f;
            _distance = Mathf.Clamp(_distance, MinDistance, MaxDistance);
        }

        Quaternion rot = Quaternion.Euler(_pitch, _yaw, 0f);

        Vector3 desiredPos = Target.position - (rot * Vector3.forward) * _distance;

        transform.position = desiredPos;
        transform.LookAt(Target.position, Vector3.up);
    }
}