using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XR_Input;
using XRControls.Effects;
using XRControls.XRInput;

namespace XRControls
{
    /// <summary>
    /// XR Rig movement controller. Continuos movement with trackpad with speed gained by acceleration.
    /// Also fades screen with custom posteffect when moving. 
    /// </summary>
    public class XRSmoothMovement : MonoBehaviour
    {
        [Header("Movement parameters")]

        [SerializeField]
        [Tooltip("Speed of speed gain.")]
        protected float acceleration = 2f;

        [SerializeField]
        [Tooltip("Highest speed in any direction")]
        protected float maxSpeed = 4f;

#if UNITY_POST_PROCESSING_STACK_V2
        [Header("Tunnel fading")]

        [SerializeField]
        [Tooltip("Maximum power value of fading posteffect")]
        protected float maxFadeValue = 0.8f;

        [SerializeField]
        [Tooltip("Movement below this speed will not activate fading")]
        protected float minSpeedToFade = 0.1f;

        [SerializeField]
        [Tooltip("How fast fading will show up and hide")]
        protected float fadeSpeed = 15f;
#endif
        [Header("Input parameters")]

        [SerializeField]
        [Tooltip("How works left trackpad. \n'None' - does nothing\n'Touching' - move by touching trackpad\n'Pressing' - move by pressing trackpad")]
        protected MovementMethod leftMovementMethod;

        [SerializeField]
        [Tooltip("How works right trackpad. \n'None' - does nothing\n'Touching' - move by touching trackpad\n'Pressing' - move by pressing trackpad")]
        protected MovementMethod rightMovementMethod;

        [SerializeField]
        [Tooltip("Will be movement processed in physics loop or in a simple update")]
        protected bool isFixedUpdate;

        /// <summary>
        /// Input collected from all trackpads. Vector clamped to maximum length of 1. After update loop it will erase so if there wasn't any input it stay at zero.
        /// </summary>
        protected Vector3 movementInputVector;

        /// <summary>
        /// Current velocity saved to support smooth acceleration and deceleration. Magnitude never shouldn't be more than max speed.
        /// </summary>
        protected Vector3 currentVelocityVector;

        #region Properties
        private XrPlayer _myXRPlayer;
        public XrPlayer MyXRPlayer
        {
            get
            {
                if (_myXRPlayer == null)
                {
                    _myXRPlayer = GetComponent<XrPlayer>();
                }
                return _myXRPlayer;
            }
        }

#if UNITY_POST_PROCESSING_STACK_V2
        private PostProcessProfile _myPostProcessProfile;
        private PostProcessProfile MyPostProcessProfile
        {
            get
            {
                if (_myPostProcessProfile == null)
                {
                    // Cache post-processing profile
                    _myPostProcessProfile = FindObjectOfType<PostProcessVolume>()?.profile;
                }
                return _myPostProcessProfile;
            }
        }

        private FloatParameter _myPostProcessFade;
        private FloatParameter MyPostProcessFade
        {
            get
            {
                if (_myPostProcessFade == null)
                {
                    // Cache FadeEffect
                    _myPostProcessFade = MyPostProcessProfile?.GetSetting<TunnelVignetteEffect>()?.fadePower;
                }
                return _myPostProcessFade;
            }
        }
#endif
        #endregion

        // Use this for initialization
        protected virtual void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes movement behaviour. Sets event listeners to input according to settings
        /// </summary>
        protected virtual void Initialize()
        {
            // Set events and behaviour for left controller
            switch (leftMovementMethod)
            {
                case MovementMethod.Touching:
                    MyXRPlayer.LeftController.MyControllerEvents.OnTrackpadTouching += OnTrackpadInput;
                    break;
                case MovementMethod.Pressing:
                    MyXRPlayer.LeftController.MyControllerEvents.OnTrackpadPressing += OnTrackpadInput;
                    break;
            }

            // Set events and behaviour for right controller
            switch (rightMovementMethod)
            {
                case MovementMethod.Touching:
                    MyXRPlayer.RightController.MyControllerEvents.OnTrackpadTouching += OnTrackpadInput;
                    break;
                case MovementMethod.Pressing:
                    MyXRPlayer.RightController.MyControllerEvents.OnTrackpadPressing += OnTrackpadInput;
                    break;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!isFixedUpdate)
            {
                // Handles movement in default game loop
                ProcessMovement(Time.deltaTime);
            }
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (isFixedUpdate)
            {
                // Handles movement in physics loop
                ProcessMovement(Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Handles movement gaining speed when there is input data and decelerating when there isn't or it became lower. 
		/// So fo example if player reaches maximum speed touching border of trackpad and then moves thumb to middle of radius, speed will
		/// decelerate up to half.
        /// </summary>
        /// <param name="deltaTime">Time span from last calculation</param>
        protected virtual void ProcessMovement(float deltaTime)
        {
            // Clamp magnitude if it became bigger than 1. So using more than one controller should not give bonus to speed. 
            movementInputVector = Vector3.ClampMagnitude(movementInputVector, 1f);
            // Calculate desired speed
            Vector3 targetSpeed = movementInputVector * maxSpeed;
            // Gain more speed up to target speed with defined acceleration
            currentVelocityVector = Vector3.MoveTowards(currentVelocityVector, targetSpeed, deltaTime * acceleration);

#if UNITY_POST_PROCESSING_STACK_V2
            // Fade in or Fade out vignette tunnel when velocity bigger than minimal value
            MyPostProcessFade.value = Mathf.Lerp(
                MyPostProcessFade.value,
                currentVelocityVector.sqrMagnitude > minSpeedToFade ? maxFadeValue : 0f,
                deltaTime * fadeSpeed
            );
#endif

            // Move XR Rig
            transform.position += currentVelocityVector * deltaTime;
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            // Reset input info from trackpads
            movementInputVector = Vector3.zero;
        }

        /// <summary>
        /// Event handler for trackpad input.
        /// </summary>
        /// <param name="sender">Ref to sender object</param>
        /// <param name="e">Trackpad input information</param>
        protected void OnTrackpadInput(object sender, ControllerTrackpadArgs e)
        {
            // Calculate local forward vector
            Vector3 flatForward = Vector3.ProjectOnPlane((sender as MonoBehaviour).transform.forward, Vector3.up).normalized;
            // Calculate local right vector
            Vector3 flatRight = Quaternion.Euler(0f, 90f, 0f) * flatForward;
            // Append input from current controller to other. Don't worry - it will be reset at the end of update loop
            movementInputVector += (flatForward * e.Vertical + flatRight * e.Horizontal);
        }

        /// <summary>
        /// Sets behaviour of trackpad input.
        /// </summary>
        protected enum MovementMethod
        {
            None,
            Touching,
            Pressing
        }
    }

}