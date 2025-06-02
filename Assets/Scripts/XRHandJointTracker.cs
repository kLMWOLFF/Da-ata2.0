using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR;
using System.Collections.Generic;
public class XRHandJointTracker : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    public enum Handedness { Left, Right }
    [Header("Hand Tracking Settings")]
    [SerializeField] private Handedness handToTrack = Handedness.Left;
    [SerializeField] private XRHandJointID jointToTrack = XRHandJointID.IndexTip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        foreach (var subsystem in subsystems)
        {
            if (subsystem.running)
            {
                handSubsystem = subsystem;
                break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (handSubsystem == null)
            return;
        XRHand hand = handToTrack == Handedness.Right
            ? handSubsystem.rightHand
            : handSubsystem.leftHand;
        if (!hand.isTracked)
            return;
        XRHandJoint joint = hand.GetJoint(jointToTrack);
        if (joint.TryGetPose(out Pose pose))
        {
            transform.position = pose.position;
            transform.rotation = pose.rotation;
        }
    }
}
