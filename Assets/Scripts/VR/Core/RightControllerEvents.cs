using System;
using UnityEngine;
using SDK = OVRInput;

namespace Assets.Scripts.VR.Core
{
    public class RightControllerEvents : MonoBehaviour
    {
        public Action OnButtonADown;
        public Action OnButtonAUp;
        public Action OnButtonAHoldBegin;
        public Action OnButtonAHold;
        public Action OnButtonAHoldEnd;
        public Action OnButtonATouch;

        public Action OnTriggerDown;
        public Action OnTriggerUp;
        public Action OnTriggerHoldBegin;
        public Action OnTriggerHold;
        public Action OnTriggerHoldEnd;
        public Action OnTriggerTouch;
        public Action OnTriggerNearTouch;

        public Action OnGripDown;
        public Action OnGripUp;
        public Action OnGripHoldBegin;
        public Action OnGripHold;
        public Action OnGripHoldEnd;

        private void Update()
        {
            var isButtonADown = SDK.GetDown(SDK.Button.One, SDK.Controller.RTouch); // pushing down
            var isButtonAUp = SDK.GetUp(SDK.Button.One, SDK.Controller.RTouch); // releasing
            var isButtonAHold = SDK.Get(SDK.Button.One, SDK.Controller.RTouch); // holding
            var isButtonATouch = SDK.Get(SDK.Touch.One, SDK.Controller.RTouch); // finger resting

            var isTriggerDown = SDK.GetDown(SDK.Button.PrimaryIndexTrigger, SDK.Controller.RTouch); // pushing down
            var isTriggerUp = SDK.GetUp(SDK.Button.PrimaryIndexTrigger, SDK.Controller.RTouch); // releasing
            var isTriggerHold = SDK.Get(SDK.Button.PrimaryIndexTrigger, SDK.Controller.RTouch); // holding
            var isTriggerTouch = SDK.Get(SDK.Touch.PrimaryIndexTrigger, SDK.Controller.RTouch); // finger resting
            var isTriggerNearTouch = SDK.Get(SDK.NearTouch.PrimaryIndexTrigger, SDK.Controller.RTouch); // finger hovering

            var isGripDown = SDK.GetDown(SDK.Button.PrimaryHandTrigger, SDK.Controller.RTouch); // pushing down
            var isGripUp = SDK.GetUp(SDK.Button.PrimaryHandTrigger, SDK.Controller.RTouch); // releasing
            var isGripHold = SDK.Get(SDK.Button.PrimaryHandTrigger, SDK.Controller.RTouch); // holding

            // BUTTON

            if (isButtonADown)
            {
                OnButtonADown?.Invoke();
                OnButtonAHoldBegin?.Invoke();
            }
            if (isButtonAUp)
            {
                OnButtonAUp?.Invoke();
                OnButtonAHoldEnd?.Invoke();
            }
            if (isButtonAHold)
                OnButtonAHold?.Invoke();
            if (isButtonATouch)
                OnButtonATouch?.Invoke();

            // TRIGGER

            if (isTriggerDown)
            {
                OnTriggerDown?.Invoke();
                OnTriggerHoldBegin?.Invoke();
            }
            if (isTriggerUp)
            {
                OnTriggerUp?.Invoke();
                OnTriggerHoldEnd?.Invoke();
            }
            if (isTriggerHold)
                OnTriggerHold?.Invoke();
            if (isTriggerTouch)
                OnTriggerTouch?.Invoke();
            if (isTriggerNearTouch)
                OnTriggerNearTouch?.Invoke();

            // GRIP

            if (isGripDown)
            {
                OnGripDown?.Invoke();
                OnGripHoldBegin?.Invoke();
            }
            if (isGripUp)
            {
                OnGripUp?.Invoke();
                OnGripHoldEnd?.Invoke();
            }
            if (isGripHold)
                OnGripHold?.Invoke();
        }
    }
}