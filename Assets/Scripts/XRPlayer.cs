using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;
using XRControls.XRInput;

namespace XRControls
{
    public class XRPlayer : MonoBehaviour
    {

        #region Properties
        private Transform _head;
        public Transform Head
        {
            get
            {
                if (_head == null)
                {
                    TrackedPoseDriver[] trackedPoses = GetComponentsInChildren<TrackedPoseDriver>();
                    for (int i = 0; i < trackedPoses.Length; i++)
                    {
                        if (trackedPoses[i].poseSource == TrackedPoseDriver.TrackedPose.Head)
                        {
                            _head = trackedPoses[i].transform;
                            break;
                        }
                    }
                }
                return _head;
            }
        }

        private XRController _leftController;
        public XRController LeftController
        {
            get
            {
                if (_leftController == null)
                {
                    TrackedPoseDriver[] trackedPoses = GetComponentsInChildren<TrackedPoseDriver>();
                    for (int i = 0; i < trackedPoses.Length; i++)
                    {
                        if (trackedPoses[i].poseSource == TrackedPoseDriver.TrackedPose.LeftPose)
                        {
                            _leftController = trackedPoses[i].GetComponent<XRController>();
                        }
                    }
                }
                return _leftController;
            }
        }

        private XRController _rightController;
        public XRController RightController
        {
            get
            {
                if (_rightController == null)
                {
                    TrackedPoseDriver[] trackedPoses = GetComponentsInChildren<TrackedPoseDriver>();
                    for (int i = 0; i < trackedPoses.Length; i++)
                    {
                        if (trackedPoses[i].poseSource == TrackedPoseDriver.TrackedPose.RightPose)
                        {
                            _rightController = trackedPoses[i].GetComponent<XRController>();
                        }
                    }
                }
                return _rightController;
            }
        }
        #endregion

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Debug.Log("model: " + XRDevice.model + ". name: " + XRSettings.loadedDeviceName);
            XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        }
    }
}