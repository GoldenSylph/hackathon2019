using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

namespace XRControls.XRInput
{
    [RequireComponent(typeof(TrackedPoseDriver))]
    public class XRController : MonoBehaviour
    {
        [System.NonSerialized]
        public bool isRight;

        #region Properties

        public bool IsRight
        {
            get
            {
                return MyTrackedPoseDriver.poseSource == TrackedPoseDriver.TrackedPose.RightPose;
            }
        }

        private XRControllerEvents _myControllerEvents;
        public XRControllerEvents MyControllerEvents
        {
            get
            {
                if (_myControllerEvents == null)
                {
                    _myControllerEvents = GetComponent<XRControllerEvents>();
                }
                return _myControllerEvents;
            }
        }

        private TrackedPoseDriver _myTrackedPoseDriver;
        private TrackedPoseDriver MyTrackedPoseDriver
        {
            get
            {
                if (_myTrackedPoseDriver == null)
                {
                    _myTrackedPoseDriver = GetComponent<TrackedPoseDriver>();
                }

                return _myTrackedPoseDriver;
            }
        }
        #endregion

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            isRight = MyTrackedPoseDriver.poseSource == TrackedPoseDriver.TrackedPose.RightPose;
        }
    }
}
