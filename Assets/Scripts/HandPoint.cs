using Oculus.Interaction.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoint
{
    /** thumb **/
    public Vector3 thumbTip, thumbDirection;
    public bool thumbPinching;
    /** index **/
    public Vector3 indexTip, indexDirection;
    public bool indexPinching;
    /** middle **/
    public Vector3 middleTip, middleDirection;
    public bool middlePinching;
    /** ring **/
    public Vector3 ringTip, ringDirection;
    public bool ringPinching;
    /** pinky **/
    public Vector3 pinkyTip, pinkyDirection;
    public bool pinkyPinching;
    /** wrist **/
    //public Vector3 wristPos;

    public long time;
    public bool isPointing, isPinching, isDrawing;


    public HandPoint(OVRSkeleton h, Hand _hand, bool pinch, bool point, bool draw)
    {
        indexTip = h.Bones[20].Transform.position;
        indexPinching = _hand.GetFingerIsPinching(HandFinger.Index);
        indexDirection = h.Bones[6].Transform.position - indexTip;

        middleTip = h.Bones[21].Transform.position;
        middlePinching = _hand.GetFingerIsPinching(HandFinger.Middle);
        middleDirection = h.Bones[9].Transform.position - middleTip;

        ringTip = h.Bones[22].Transform.position;
        ringPinching = _hand.GetFingerIsPinching(HandFinger.Ring);
        ringDirection = h.Bones[12].Transform.position - ringTip;

        pinkyTip = h.Bones[23].Transform.position;
        pinkyPinching = _hand.GetFingerIsPinching(HandFinger.Pinky);
        pinkyDirection = h.Bones[16].Transform.position - pinkyTip;

        thumbTip = h.Bones[19].Transform.position;
        thumbPinching = _hand.GetFingerIsPinching(HandFinger.Thumb);
        thumbDirection = h.Bones[3].Transform.position - thumbTip;

        isPinching = pinch;
        isPointing = point;
        isDrawing = draw;
        time = DateTime.Now.Ticks - SaveData.BEGIN.Ticks;
    }
}
