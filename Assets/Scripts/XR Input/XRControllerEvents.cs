using System;
using UnityEngine;
using XRControls.XRInput;

namespace XR_Input
{
    [RequireComponent(typeof(XrController))]
    public class XrControllerEvents : MonoBehaviour
    {
        #region Properties

        private XrController myXrController;
        public XrController MyXrController
        {
            get
            {
                if (myXrController == null)
                {
                    myXrController = GetComponent<XrController>();
                }

                return myXrController;
            }
        }

        private string Side => MyXrController.isRight ? "Right " : "Left ";

        #endregion

        private ControllerTrackpadArgs trackpadArgs;
        private ControllerGripArgs gripArgs;

        public delegate void ControllerTrackpadEventHandler(object sender, ControllerTrackpadArgs e);
        public event ControllerTrackpadEventHandler OnTrackpadTouched;
        public event ControllerTrackpadEventHandler OnTrackpadTouching;
        public event ControllerTrackpadEventHandler OnTrackpadUntouched;
        public event ControllerTrackpadEventHandler OnTrackpadPressed;
        public event ControllerTrackpadEventHandler OnTrackpadPressing;
        public event ControllerTrackpadEventHandler OnTrackpadUnpressed;
        public event ControllerTrackpadEventHandler OnTrackpadMoved;

        public delegate void ControllerGripEventHandler(object sender, ControllerGripArgs e);
        public event ControllerGripEventHandler OnGripPressed;
        public event ControllerGripEventHandler OnGripReleased;

        private const float TOLERANCE = 0.000001f;
        
        #region Methods

        private void Update()
        {
            UpdateInputs();
        }

        private void UpdateInputs()
        {
            UpdateTrackpad();
            UpdateGrip();
        }

        private void UpdateGrip()
        {
            var gripPressed = Math.Abs(Input.GetAxis(Side + StaticAliases.alias_GripSqueeze) - 1) < TOLERANCE;
            var isPressChanged = GetButtonState(gripArgs.GripPress, gripPressed, out var newGripPressState);
            
            if (!isPressChanged) return;
            gripArgs.GripPress = newGripPressState;
            
            switch (gripArgs.GripPress)
            {
                
                case InputDataButtonState.Released:
                    Debug.Log(OnGripReleased);
                    OnGripReleased?.Invoke(this, gripArgs);
                    Debug.Log("Released");
                    break;
                
                case InputDataButtonState.Pressed:
                    Debug.Log(OnGripPressed);
                    OnGripPressed?.Invoke(this, gripArgs);
                    Debug.Log("Pressed");
                    break;

                case InputDataButtonState.StayReleased:
                case InputDataButtonState.StayPressed:
                    break;
                
                default:
                    break;
            }
        }
        
        private void UpdateTrackpad()
        {
            var trackpadPress = Input.GetButton(Side + StaticAliases.alias_TrackpadPress);
            var isPressChanged = GetButtonState(trackpadArgs.TrackpadPress, trackpadPress, out var newTrackpadPressState);

            var trackpadTouch = Input.GetButton(Side + StaticAliases.alias_TrackpadTouch);
            var isTouchChanged = GetButtonState(trackpadArgs.TrackpadTouch, trackpadTouch, out var newTrackpadTouchState);

            var horizontal = Input.GetAxis(Side + StaticAliases.alias_TrackpadHorizontal);
            var vertical = -Input.GetAxis(Side + StaticAliases.alias_TrackpadVertical);
            var isTrackpadMoved = (Math.Abs(horizontal - trackpadArgs.Horizontal) > TOLERANCE) | (Math.Abs(vertical - trackpadArgs.Vertical) > TOLERANCE);

            if (isPressChanged || isTouchChanged || isTrackpadMoved)
            {
                trackpadArgs.TrackpadPress = newTrackpadPressState;
                trackpadArgs.TrackpadTouch = newTrackpadTouchState;
                trackpadArgs.Horizontal = horizontal;
                trackpadArgs.Vertical = vertical;
            }

            if (isPressChanged)
            {
                switch (trackpadArgs.TrackpadPress)
                {
                    case InputDataButtonState.Pressed:
                        OnTrackpadPressed?.Invoke(this, trackpadArgs);
                        break;
                    case InputDataButtonState.Released:
                        OnTrackpadUnpressed?.Invoke(this, trackpadArgs);
                        break;
                    
                    case InputDataButtonState.StayReleased:
                    case InputDataButtonState.StayPressed:
                        break;
                    default:
                        break;
                }
            }

            if (trackpadArgs.TrackpadPress == InputDataButtonState.Pressed || trackpadArgs.TrackpadPress == InputDataButtonState.StayPressed)
            {
                OnTrackpadPressing?.Invoke(this, trackpadArgs);
            }

            if (isTouchChanged)
            {
                switch (trackpadArgs.TrackpadTouch)
                {
                    case InputDataButtonState.Pressed:
                        OnTrackpadTouched?.Invoke(this, trackpadArgs);
                        break;
                    case InputDataButtonState.Released:
                        OnTrackpadUntouched?.Invoke(this, trackpadArgs);
                        break;
                    
                    case InputDataButtonState.StayReleased:
                    case InputDataButtonState.StayPressed:
                        break;
                    default:
                        break;
                }
            }

            if (trackpadArgs.TrackpadTouch == InputDataButtonState.Pressed || trackpadArgs.TrackpadTouch == InputDataButtonState.StayPressed)
            {
                OnTrackpadTouching?.Invoke(this, trackpadArgs);
            }

            if (isTrackpadMoved)
            {
                OnTrackpadMoved?.Invoke(this, trackpadArgs);
            }
        }

        private static bool GetButtonState(InputDataButtonState previousState, bool isButtonPressedNow, out InputDataButtonState newState)
        {
            var isChanged = false;
            newState = previousState;
            switch (previousState)
            {
                case InputDataButtonState.StayReleased when isButtonPressedNow:
                    newState = InputDataButtonState.Pressed;
                    isChanged = true;
                    break;
                case InputDataButtonState.Pressed when isButtonPressedNow:
                    newState = InputDataButtonState.StayPressed;
                    isChanged = true;
                    break;
                case InputDataButtonState.Pressed:
                    newState = InputDataButtonState.Released;
                    isChanged = true;
                    break;
                case InputDataButtonState.StayPressed when !isButtonPressedNow:
                    newState = InputDataButtonState.Released;
                    isChanged = true;
                    break;
                case InputDataButtonState.Released when isButtonPressedNow:
                    newState = InputDataButtonState.Pressed;
                    isChanged = true;
                    break;
                case InputDataButtonState.Released:
                    newState = InputDataButtonState.StayReleased;
                    isChanged = true;
                    break;
                default:
                    break;
            }

            return isChanged;
        }
        #endregion
    }

    public struct ControllerGripArgs
    {
        public InputDataButtonState GripPress;
    }
    
    public struct ControllerTrackpadArgs
    {
        public InputDataButtonState TrackpadTouch;
        public InputDataButtonState TrackpadPress;
        public float Vertical;
        public float Horizontal;
    }

    public enum InputDataButtonState
    {
        StayReleased,
        Pressed,
        StayPressed,
        Released
    }
}
