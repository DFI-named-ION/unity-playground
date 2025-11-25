using System;
using UnityEngine;
using SDK = OVRInput;

namespace Assets.Scripts.VR.Core
{
    public class LeftControllerEvents : MonoBehaviour
    {
        private float deadZone = 0.15f;

        public Action OnStickDown;
        public Action OnStickUp;
        public Action OnStickHoldBegin;
        public Action OnStickHold;
        public Action OnStickHoldEnd;
        public Action OnStickTouch;
        public Action<Vector2> OnStickAxisChange;
        public Action<float> OnStickLeanUp;
        public Action<float> OnStickLeanDown;
        public Action<float> OnStickLeanLeft;
        public Action<float> OnStickLeanRight;

        private void Update()
        {
            var isStickDown = SDK.GetDown(SDK.Button.PrimaryThumbstick, SDK.Controller.LTouch);
            var isStickUp = SDK.GetUp(SDK.Button.PrimaryThumbstick, SDK.Controller.LTouch);
            var isStickHold = SDK.Get(SDK.Button.PrimaryThumbstick, SDK.Controller.LTouch);
            var isStickTouch = SDK.Get(SDK.Touch.PrimaryThumbstick, SDK.Controller.LTouch);

            if (isStickDown)
            {
                OnStickDown?.Invoke();
                OnStickHoldBegin?.Invoke();
            }
            if (isStickUp)
            {
                OnStickUp?.Invoke();
                OnStickHoldEnd?.Invoke();
            }
            if (isStickHold)
                OnStickHold?.Invoke();
            if (isStickTouch)
                OnStickTouch?.Invoke();

            Vector2 axis = SDK.Get(SDK.Axis2D.PrimaryThumbstick, SDK.Controller.LTouch);

            if (axis.magnitude < deadZone)
                axis = Vector2.zero;
            
            OnStickAxisChange?.Invoke(axis);

            if (axis == Vector2.zero) return;

            if (Mathf.Abs(axis.x) > Mathf.Abs(axis.y))
            {
                if (axis.y > 0f)
                    OnStickLeanUp?.Invoke(axis.y);
                else
                    OnStickLeanDown?.Invoke(-axis.y);
            }
            else
            {
                if (axis.x > 0f)
                    OnStickLeanRight?.Invoke(axis.x);
                else
                    OnStickLeanLeft?.Invoke(-axis.x);
            }
        }
    }
}