using System;
using UnityEngine;

namespace XRControls.XRInput
{
    [RequireComponent(typeof(XRController))]
    public class XRControllerEvents : MonoBehaviour
    {
        #region Properties

        private XRController _myXRController;
        private XRController MyXRController
        {
            get
            {
                if (_myXRController == null)
                {
                    _myXRController = GetComponent<XRController>();
                }

                return _myXRController;
            }
        }

        private string Side
        {
            get
            {
                return MyXRController.isRight ? "Right " : "Left ";
            }
        }
        #endregion

        private ControllerTrackpadArgs trackpadArgs;

        public delegate void ControllerTrackpadEventHandler(object sender, ControllerTrackpadArgs e);
        public event ControllerTrackpadEventHandler OnTrackpadTouched;
        public event ControllerTrackpadEventHandler OnTrackpadTouching;
        public event ControllerTrackpadEventHandler OnTrackpadUntouched;
        public event ControllerTrackpadEventHandler OnTrackpadPressed;
        public event ControllerTrackpadEventHandler OnTrackpadPressing;
        public event ControllerTrackpadEventHandler OnTrackpadUnpressed;
        public event ControllerTrackpadEventHandler OnTrackpadMoved;

        #region Methods
        // Update is called once per frame
        void Update()
        {
            UpdateInputs();
        }

        private void UpdateInputs()
        {
            UpdateTrackpad();

            // Debug.Log("Grip: " + Input.GetAxis(Side + StaticAliases.alias_GripSqueeze));
            // if (Input.GetButton(Side + StaticAliases.alias_TrackpadTouch))
            // {
            //     ControllerTrackpadArgs cta = new ControllerTrackpadArgs();
            //     cta.horizontal = Input.GetAxis(Side + StaticAliases.alias_TrackpadHorizontal);
            //     cta.vertical = -Input.GetAxis(Side + StaticAliases.alias_TrackpadVertical);
            //     OnTrackpadTouched?.Invoke(this, cta);
            // }
        }

        private void UpdateTrackpad()
        {
            bool trackpadPress = Input.GetButton(Side + StaticAliases.alias_TrackpadPress);
            InputDataButtonState newTrackpadPressState;
            bool isPressChanged = GetButtonState(trackpadArgs.trackpadPress, trackpadPress, out newTrackpadPressState);

            bool trackpadTouch = Input.GetButton(Side + StaticAliases.alias_TrackpadTouch);
            InputDataButtonState newTrackpadTouchState;
            bool isTouchChanged = GetButtonState(trackpadArgs.trackpadTouch, trackpadTouch, out newTrackpadTouchState);

            float horizontal = Input.GetAxis(Side + StaticAliases.alias_TrackpadHorizontal);
            float vertical = -Input.GetAxis(Side + StaticAliases.alias_TrackpadVertical);
            bool isTrackpadMoved = (horizontal != trackpadArgs.horizontal) | (vertical != trackpadArgs.vertical);

            if (isPressChanged || isTouchChanged || isTrackpadMoved)
            {
                trackpadArgs.trackpadPress = newTrackpadPressState;
                trackpadArgs.trackpadTouch = newTrackpadTouchState;
                trackpadArgs.horizontal = horizontal;
                trackpadArgs.vertical = vertical;
            }

            if (isPressChanged)
            {
                if (trackpadArgs.trackpadPress == InputDataButtonState.Pressed)
                {
                    OnTrackpadPressed?.Invoke(this, trackpadArgs);
                }
                else if (trackpadArgs.trackpadPress == InputDataButtonState.Released)
                {
                    OnTrackpadUnpressed?.Invoke(this, trackpadArgs);
                }
            }

            if (trackpadArgs.trackpadPress == InputDataButtonState.Pressed || trackpadArgs.trackpadPress == InputDataButtonState.StayPressed)
            {
                OnTrackpadPressing?.Invoke(this, trackpadArgs);
            }

            if (isTouchChanged)
            {
                if (trackpadArgs.trackpadTouch == InputDataButtonState.Pressed)
                {
                    OnTrackpadTouched?.Invoke(this, trackpadArgs);
                }
                else if (trackpadArgs.trackpadTouch == InputDataButtonState.Released)
                {
                    OnTrackpadUntouched?.Invoke(this, trackpadArgs);
                }
            }

            if (trackpadArgs.trackpadTouch == InputDataButtonState.Pressed || trackpadArgs.trackpadTouch == InputDataButtonState.StayPressed)
            {
                OnTrackpadTouching?.Invoke(this, trackpadArgs);
            }

            if (isTrackpadMoved)
            {
                OnTrackpadMoved?.Invoke(this, trackpadArgs);
            }
        }

        public bool GetButtonState(InputDataButtonState previousState, bool isButtonPressedNow, out InputDataButtonState newState)
        {
            bool isChanged = false;
            newState = previousState;

            if (previousState == InputDataButtonState.StayReleased && isButtonPressedNow)
            {
                newState = InputDataButtonState.Pressed;
                isChanged = true;
            }
            else if (previousState == InputDataButtonState.Pressed && isButtonPressedNow)
            {
                newState = InputDataButtonState.StayPressed;
                isChanged = true;
            }
            else if (previousState == InputDataButtonState.Pressed && !isButtonPressedNow)
            {
                newState = InputDataButtonState.Released;
                isChanged = true;
            }
            else if (previousState == InputDataButtonState.StayPressed && !isButtonPressedNow)
            {
                newState = InputDataButtonState.Released;
                isChanged = true;
            }
            else if (previousState == InputDataButtonState.Released && isButtonPressedNow)
            {
                newState = InputDataButtonState.Pressed;
                isChanged = true;
            }
            else if (previousState == InputDataButtonState.Released && !isButtonPressedNow)
            {
                newState = InputDataButtonState.StayReleased;
                isChanged = true;
            }

            return isChanged;
        }
        #endregion
    }

    public struct ControllerTrackpadArgs
    {
        public InputDataButtonState trackpadTouch;
        public InputDataButtonState trackpadPress;
        public float vertical;
        public float horizontal;
    }

    public enum InputDataButtonState
    {
        StayReleased,
        Pressed,
        StayPressed,
        Released
    }
}
