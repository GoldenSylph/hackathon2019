using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;
using XR_Input;

public class XrPlayer : MonoBehaviour
{
    private static XrPlayer _player; 
    public static XrPlayer Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<XrPlayer>();
            }
            return _player;
        }
    }

    #region Properties
    private Transform head;
    public Transform Head
    {
        get
        {
            if (head != null) return head;
            var trackedPoses = GetComponentsInChildren<TrackedPoseDriver>();
            foreach (var t in trackedPoses)
            {
                if (t.poseSource != TrackedPoseDriver.TrackedPose.Head) continue;
                head = t.transform;
                break;
            }
            return head;
        }
    }

    private XrController leftController;
    public XrController LeftController
    {
        get
        {
            if (leftController != null) return leftController;
            var trackedPoses = GetComponentsInChildren<TrackedPoseDriver>();
            foreach (var t in trackedPoses)
            {
                if (t.poseSource == TrackedPoseDriver.TrackedPose.LeftPose)
                {
                    leftController = t.GetComponent<XrController>();
                }
            }
            return leftController;
        }
    }

    private XrController rightController;
    public XrController RightController
    {
        get
        {
            if (rightController != null) return rightController;
            var trackedPoses = GetComponentsInChildren<TrackedPoseDriver>();
            foreach (var t in trackedPoses)
            {
                if (t.poseSource == TrackedPoseDriver.TrackedPose.RightPose)
                {
                    rightController = t.GetComponent<XrController>();
                }
            }
            return rightController;
        }
    }
    #endregion

    private void Start()
    {
        Initialize();
    }

    private static void Initialize()
    {
        // Debug.Log("model: " + XRDevice.model + ". name: " + XRSettings.loadedDeviceName);
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
    }
}