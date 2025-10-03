using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoaMovement : MonoBehaviour
{
    public float MoveForce = 10f;
    public float MaxSpeed = 5f;
    public float JumpForce = 5f;
    public bool UseBraking = true;
    public float BrakingFactor = 0.995f;
    public Transform CameraTransform;

    private Rigidbody _rb;

    private bool _jumpRequested = false;
    private HashSet<Collider> _contacts = new HashSet<Collider>();

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null)
            _contacts.Add(collision.collider);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider != null)
            _contacts.Remove(collision.collider);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            _jumpRequested = true;
    }

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;

        move.x = (Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f);
        move.y = (Keyboard.current.wKey.isPressed ? 1f : 0f) - (Keyboard.current.sKey.isPressed ? 1f : 0f);

        Vector3 input = new Vector3(move.x, 0f, move.y);

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        if (input.sqrMagnitude > 0f)
        {
            Vector3 camForward = CameraTransform.forward;
            Vector3 camRight = CameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * input.z + camRight * input.x;

            Vector3 force = moveDir * MoveForce;
            _rb.AddForce(force, ForceMode.Force);
        }
        else
        {
            Vector3 horiz = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            horiz *= BrakingFactor;
            _rb.linearVelocity = new Vector3(horiz.x, _rb.linearVelocity.y, horiz.z);
        }

        Vector3 velXZ = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        if (velXZ.magnitude > MaxSpeed && MaxSpeed > 0f)
        {
            Vector3 clamped = velXZ.normalized * MaxSpeed;
            _rb.linearVelocity = new Vector3(clamped.x, _rb.linearVelocity.y, clamped.z);
        }

        _contacts.RemoveWhere(c => c == null || !c.enabled || !c.gameObject.activeInHierarchy);

        if (_jumpRequested && _contacts.Count > 0)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }
}