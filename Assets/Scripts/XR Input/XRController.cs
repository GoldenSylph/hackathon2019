using UnityEngine;
using UnityEngine.SpatialTracking;

namespace XR_Input
{
    [RequireComponent(typeof(TrackedPoseDriver))]
    public class XrController : MonoBehaviour
    {
        [System.NonSerialized]
        public bool isRight;

        #region Properties

        public bool IsRight => MyTrackedPoseDriver.poseSource == TrackedPoseDriver.TrackedPose.RightPose;

        private XrControllerEvents myControllerEvents;
        public XrControllerEvents MyControllerEvents
        {
            get
            {
                if (!myControllerEvents)
                {
                    myControllerEvents = GetComponent<XrControllerEvents>();
                }
                return myControllerEvents;
            }
        }

        private TrackedPoseDriver myTrackedPoseDriver;
        private TrackedPoseDriver MyTrackedPoseDriver
        {
            get
            {
                if (!myTrackedPoseDriver)
                {
                    myTrackedPoseDriver = GetComponent<TrackedPoseDriver>();
                }

                return myTrackedPoseDriver;
            }
        }
        #endregion

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            isRight = MyTrackedPoseDriver.poseSource == TrackedPoseDriver.TrackedPose.RightPose;
        }
    }
}
