/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrimaryHandPinchInteraction : MonoBehaviour
{
    public OVRHand hand;
    public Transform camera;
    public OVRSkeleton skeleton;
    public GameObject label;
    
    public enum LeftActions
    {
        Undo,
        Redo, 
        Duplicate,
        Settings
    }
    public enum Finger
    {
        Index, 
        Middle,
        Ring, 
        Pinky
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        // Get finger pinch values
        (float pinchStrength, Finger finger) = getPinchingFinger();
        if (pinchStrength > 0)
        {
            showLabelOnFinger(finger);
            if (pinchStrength == 1)
            {
                highlightLabel();
            } else
            {
                dimLabel();
            }
        } else
        {
            hideLabel();
        }

    }

    private void highlightLabel()
    {
        Image img = label.GetComponent<Image>();
        Color newColor = img.color;
        newColor.a = 1.0f;
        img.color = newColor;
    }

    private void dimLabel()
    {
        Image img = label.GetComponent<Image>();
        Color newColor = img.color;
        newColor.a = 0.5f;
        img.color = newColor;
    }

    private void hideLabel()
    {
        label.SetActive(false);
    }

    private void showLabelOnFinger(Finger finger)
    {
        label.SetActive(true);
        OVRSkeleton.BoneId[] boneIds = { 
            OVRSkeleton.BoneId.Hand_IndexTip, 
            OVRSkeleton.BoneId.Hand_MiddleTip, 
            OVRSkeleton.BoneId.Hand_RingTip, 
            OVRSkeleton.BoneId.Hand_PinkyTip };

        OVRSkeleton.BoneId targetFingerBoneId = boneIds[(int)finger];

        Transform targetTipTransform = getFingerTipTransform(targetFingerBoneId);
        Transform thumbTipTransform = getFingerTipTransform(OVRSkeleton.BoneId.Hand_ThumbTip);

        Vector3 labelPosition = (targetTipTransform.position + thumbTipTransform.position) / 2f;

        // Calculate the direction from the average position to the camera
        Vector3 directionToCamera = camera.position - labelPosition;
        directionToCamera.Normalize();
        labelPosition = labelPosition + directionToCamera * distanceToMove;

        label.GetComponentInChildren<TMP_Text>().text = ((LeftActions)((int)finger)).ToString();
        label.transform.position = labelPosition;
        label.transform.rotation = Quaternion.LookRotation(label.transform.position - camera.position);

    }

    private (float, Finger) getPinchingFinger() {
        float[] pinchStrengths = new float[4];
        OVRHand.HandFinger[] fingers = new OVRHand.HandFinger[4];
        
        // Populate the array with fingers
        fingers[0] = OVRHand.HandFinger.Index;
        fingers[1] = OVRHand.HandFinger.Middle;
        fingers[2] = OVRHand.HandFinger.Ring;
        fingers[3] = OVRHand.HandFinger.Pinky;

        // Populate the array with pinch strengths
        pinchStrengths[0] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        pinchStrengths[1] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
        pinchStrengths[2] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
        // pinchStrengths[3] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky);

        // Find the maximum value and its index in the array
        float maxValue = pinchStrengths[0];
        int maxIndex = 0;

        for (int i = 1; i < pinchStrengths.Length; i++)
        {
            if (pinchStrengths[i] > maxValue)
            {
                maxValue = pinchStrengths[i];
                maxIndex = i;
            }
        }

        return (maxValue, (Finger)maxIndex);
    }


    Transform getFingerTipTransform(OVRSkeleton.BoneId boneId) {
        foreach (var b in skeleton.Bones) {
            if (b.Id == boneId) {
                return b.Transform;
            }
        }
        return null;
    }
}

*/