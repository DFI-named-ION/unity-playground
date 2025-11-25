using Assets.Scripts.VR.Core;
using Oculus.Interaction;
using System.Collections;
using UnityEngine;

public class AlyxGrabbable : MonoBehaviour
{
    public float ArcHeight = 0.5f;

    private RayInteractable _interactable;
    private Rigidbody _rb;

    public RightControllerEvents _controllerEvents;
    private bool _isGripHeld = false;

    private void Awake()
    {
        _interactable = GetComponentInChildren<RayInteractable>();
        _rb = GetComponent<Rigidbody>();

        _controllerEvents.OnGripHoldBegin += OnGripHoldBegin;
        _controllerEvents.OnGripHoldEnd += OnGripHoldEnd;

        _interactable.WhenSelectingInteractorRemoved.Action += OnInteractorRemoved;
    }

    private void OnGripHoldBegin()
    {
        _isGripHeld = true;
    }

    private void OnGripHoldEnd()
    {
        _isGripHeld = false;
    }

    private void OnInteractorRemoved(RayInteractor interactor)
    {
        if (!_isGripHeld) return;

        StartCoroutine(MakeYeet(interactor));
    }

    private IEnumerator MakeYeet(RayInteractor interactor)
    {
        var startPos = _rb.position;
        var endPos = interactor.transform.position;

        yield return new WaitForFixedUpdate();

        var dynamicHeight = ComputeArcHeight(startPos, endPos);

        if (SolveBallisticArcHeight(startPos, endPos, dynamicHeight, out var velocity) && _isGripHeld)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.linearVelocity = velocity;
        }
    }

    private bool SolveBallisticArcHeight(Vector3 start, Vector3 target, float maxHeight, out Vector3 initialVelocity)
    {
        initialVelocity = Vector3.zero;

        Vector3 g = Physics.gravity;
        float gY = g.y;
        if (Mathf.Approximately(gY, 0f)) return false;

        float dy = target.y - start.y;
        float h = maxHeight;

        float vy = Mathf.Sqrt(-2f * gY * h);

        float a = 0.5f * gY;
        float b = vy;
        float c = -dy;

        float discriminant = b * b - 4f * a * c;
        if (discriminant < 0f)
            return false;

        float sqrtDisc = Mathf.Sqrt(discriminant);
        float T1 = (-b + sqrtDisc) / (2f * a);
        float T2 = (-b - sqrtDisc) / (2f * a);

        float T = Mathf.Max(T1, T2);
        if (T <= 0f) return false;

        Vector3 horizontalDisplacement = new Vector3(
            target.x - start.x,
            0f,
            target.z - start.z
        );

        Vector3 vXZ = horizontalDisplacement / T;

        initialVelocity = new Vector3(vXZ.x, vy, vXZ.z);
        return true;
    }

    private float ComputeArcHeight(Vector3 start, Vector3 target)
    {
        float horizontalDist = new Vector2(target.x - start.x, target.z - start.z).magnitude;
        float highestY = Mathf.Max(start.y, target.y);
        float offsetFromHighest = ArcHeight + horizontalDist * ArcHeight * 0.5f;
        float apexY = highestY + offsetFromHighest;
        float h = apexY - start.y;

        float minH = 0.05f;
        float maxH = ArcHeight * 4f;

        return Mathf.Clamp(h, minH, maxH);
    }
}