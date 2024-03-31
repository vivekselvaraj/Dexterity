using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SecondaryHandPinchInteraction : MonoBehaviour
{
    public OVRHand hand;
    public Transform camera;
    public OVRSkeleton skeleton;
    public TMP_Text dialogTitle;
    public TMP_Text dialogBody;
    public GameObject label;
    public float distanceToMove = 0.02f;

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

    // Update is called once per frame
    void Update2()
    {
        // pinchRecognition();
        bool isIndexFingerPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        float pinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        string text = dialogTitle.text;
        
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index)) {
            text = "Index Finger Pinched!";
        } 
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)) {
            text = "Middle Finger Pinched!";
        }
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Ring)) {
            text = "Ring Finger Pinched!";
        }
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky)) {
            text = "Pinky Finger Pinched!";
        }
        dialogTitle.SetText(text);
        dialogBody.SetText(pinchStrength.ToString());

        Transform handIndexTipTransform;
        Transform handThumbTipTransform;

        if (pinchStrength > 0) {
            // bring the pinchSphere between the two fingers - Thumb and finger. 
            handIndexTipTransform = getFingerTipTransform(OVRSkeleton.BoneId.Hand_IndexTip);
            handThumbTipTransform = getFingerTipTransform(OVRSkeleton.BoneId.Hand_ThumbTip);

            Vector3 averagePosition = (handIndexTipTransform.position + handThumbTipTransform.position) / 2f;

            // Calculate the direction from the average position to the camera
            Vector3 directionToCamera = camera.position - averagePosition;
            directionToCamera.Normalize();
            Vector3 newPosition = averagePosition + directionToCamera * distanceToMove;
            Debug.Log(label.GetComponentInChildren<TMP_Text>().text);
            // pinchSphere.transform.position = newPosition;
            label.transform.position = newPosition;
            label.transform.rotation = Quaternion.LookRotation(newPosition - camera.position);
            label.transform.rotation = Quaternion.LookRotation(label.transform.position - camera.position);

        }
    }

    void Update()
    {
        // Update2();
        // Get finger pinch values
        (float pinchStrength, Finger finger) = getPinchingFinger();
        dialogTitle.text = finger.ToString();
        dialogBody.text = pinchStrength.ToString();
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
        label.GetComponent<FadeInTransition>().fadeInStarted = false;
        img.color = newColor;
    }

    private void dimLabel()
    {
        if (!label.GetComponent<FadeInTransition>().fadeInStarted) {
            Image img = label.GetComponent<Image>();
            Color newColor = img.color;
            newColor.a = 0.5f;
            img.color = newColor;
        }
    }

    private void hideLabel()
    {
        label.SetActive(false);
    }

    private void showLabelOnFinger(Finger finger)
    {
        if (!label.activeSelf) {
            label.SetActive(true);
            label.GetComponent<FadeInTransition>().TriggerFadeIn();
        }
        //if (!label.GetComponent<FadeInTransition>().fadeInStarted)
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
