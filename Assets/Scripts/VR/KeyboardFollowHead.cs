using UnityEngine;

public class KeyboardFollowHead : MonoBehaviour
{
    public OVRVirtualKeyboard Keyboard;
    public Transform Head;
    public float Distance = 1.25f;
    public float VerticalOffset = -0.5f;

    public float RecenterMoveThreshold = 0.6f;
    public float RecenterAngleThreshold = 35f;

    private Vector3 _lastAnchorPos;
    private Quaternion _lastAnchorRot;
    private bool _initialized;

    private void Update()
    {
        if (Keyboard == null || Head == null) return;

        if (!_initialized)
        {
            _lastAnchorPos = Head.position;
            _lastAnchorRot = Head.rotation;
            _initialized = true;
        }

        float moveDist = Vector3.Distance(Head.position, _lastAnchorPos);
        float angle = Quaternion.Angle(Head.rotation, _lastAnchorRot);

        if (moveDist > RecenterMoveThreshold || angle > RecenterAngleThreshold)
        {
            RecenterNow();
            _lastAnchorPos = Head.position;
            _lastAnchorRot = Head.rotation;
        }
    }

    public void RecenterNow()
    {
        if (Keyboard == null || Head == null) return;

        Vector3 targetPos = Head.position + Head.forward * Distance;
        targetPos += Head.up * VerticalOffset;

        Vector3 toHead = Head.position - targetPos;
        if (toHead.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(toHead);
        targetRot *= Quaternion.Euler(0f, 180f, 0f);

        Keyboard.transform.SetPositionAndRotation(targetPos, targetRot);
        Keyboard.UseSuggestedLocation(OVRVirtualKeyboard.KeyboardPosition.Custom);
    }
}