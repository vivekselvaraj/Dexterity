using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryHandUI : MonoBehaviour
{

    public OVRHand hand;
    public OVRSkeleton skeleton;
    public GameObject cubePrefab;
    public GameObject spherePrefab;

    public float scaleMax = 0.01f;
    public float scaleMin = 0.0001f;
    public float pinchFactor = 0.8f;
    public float forwardOffset = 0.1f;

    private GameObject cubeUI;
    private GameObject sphereUI;
    private GameObject middleFingerUI;


    // Start is called before the first frame update
    void Start()
    {
        cubeUI = Instantiate(cubePrefab);
        cubeUI.SetActive(false);
        sphereUI = Instantiate(spherePrefab);
        sphereUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        PrimaryCreateFingerInteraction primaryCreateFingerInteraction = GetComponent<PrimaryCreateFingerInteraction>();
        bool isCube = primaryCreateFingerInteraction.cubeSelected;
     
        if (isCube)
        {
            middleFingerUI = cubeUI;
        }
        else
        {
            middleFingerUI = sphereUI;

        }
        if (hand.IsTracked) {
            bool isMiddleFingerPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Middle);
            float pinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
            if (pinchStrength > pinchFactor)
            {
                // hide UI
                middleFingerUI.SetActive(false);
            } else
            {
                // Adjust the range (1f, 0.1f) as needed
                float scaleFactor = Mathf.Lerp(scaleMax, scaleMin, pinchStrength);
                // Apply the scale to cubeUI
                middleFingerUI.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

                // show a small cube on the index finger
                Transform middleTipTransform = getFingerTipTransform(OVRSkeleton.BoneId.Hand_MiddleTip);
                Transform thumbTipTransform = getFingerTipTransform(OVRSkeleton.BoneId.Hand_ThumbTip);
                Vector3 middleTipPosition = middleTipTransform.position;
                Vector3 uiPosition = middleTipPosition;
               /* if (pinchStrength > 0)
                {
                    Vector3 thumbTipPosition = thumbTipTransform.position;
                    uiPosition = (middleTipPosition + thumbTipPosition) / 2f;
                }*/
                uiPosition += middleTipTransform.right * forwardOffset;
                middleFingerUI.SetActive(true);
                middleFingerUI.transform.position = uiPosition;
            }
        }
    }


    // Returns the transform of a bone
    Transform getFingerTipTransform(OVRSkeleton.BoneId boneId)
    {
        foreach (var b in skeleton.Bones)
        {
            if (b.Id == boneId)
            {
                return b.Transform;
            }
        }
        return null;
    }
}
