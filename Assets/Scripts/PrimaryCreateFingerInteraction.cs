

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryCreateFingerInteraction : MonoBehaviour
{

    public OVRHand rightHand;
    public OVRHand leftHand;
    public OVRSkeleton rightSkeleton;
    public GameObject cubePrefab;
    private float pinchHoldDuration = 0.2f; // Duration to check if it is pinch or pinch and hold
    private float startTime = 0f;
    private GameObject cube;
    private bool pinchStarted; 
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentPosition;

    // Start is called before the first frame 
    void Start()
    {
        pinchStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool isFingerPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle);
        
        currentPosition = getAverageFingerPosition();

        if (isFingerPinching) { // Finger pinching
            if (pinchStarted) { // Continue dragging and scaling cube
                endPosition = currentPosition;
                if (Time.time - startTime >= pinchHoldDuration) {
                    Debug.Log("Drag Detected");
                    updateCube();
                }
            } else { // Start dragging 
                pinchStarted = true;
                startTime = Time.time;
                startPosition = currentPosition;
            }
        }
        if (!isFingerPinching) { // Finger not pinching
            if (pinchStarted) { // End pinch and unassign cube
                pinchStarted = false;
                if (Time.time - startTime < pinchHoldDuration) {
                    Debug.Log("Pinch Detected");
                } else {
                    endPosition = currentPosition;
                    updateCube();
                    setTransparency(cube, 1.0f);
                    cube = null; // Created - setting it to null    
                }
                
            } else { // Do nothing
               // Die on a big hill of gameobjects
            }
        }
    }

    void updateCube()
    {
        Vector3 center = (startPosition + endPosition) / 2f;
        if (cube == null)
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube = Instantiate(cubePrefab, center, Quaternion.identity);
            setRandomColor(cube);
            setTransparency(cube, 0.5f);

        }

        cube.transform.position = center;

        // Calculate the size of the cube
        // If any of the dimension is less than minSize, update it
        float minSize = 0.01f;
        Vector3 size = new Vector3(
            Mathf.Max(minSize, Mathf.Abs(startPosition.x - endPosition.x)),
            Mathf.Max(minSize, Mathf.Abs(startPosition.y - endPosition.y)),
            Mathf.Max(minSize, Mathf.Abs(startPosition.z - endPosition.z))
        );

        if (leftHand.IsTracked && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index)) {
            float maxSize = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
            size = new Vector3(maxSize, maxSize, maxSize);
        }

        cube.transform.localScale = size;
    }

    void setTransparency(GameObject gameObject, float transparency) {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Material material = renderer.material;
        Color color = material.color;
        color.a = transparency;
        material.color = color;
    }

    void setRandomColor(GameObject gameObject) {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        float randomRed = 1.0f; // constrain to higher values for lighter colors
        float randomGreen = Random.Range(0.5f, 1.0f);
        float randomBlue = Random.Range(0.5f, 1.0f);

        // Create a new Color using the random values
        Color randomColor = new Color(randomRed, randomGreen, randomBlue);
        renderer.material.color = randomColor;
    }

    Vector3 getAverageFingerPosition() {
        // New lines added below this point
        Transform handFingerTipTransform = null, handThumbTipTransform = null;
        foreach (var b in rightSkeleton.Bones)
        {
            if (b.Id == OVRSkeleton.BoneId.Hand_MiddleTip)
            {
                handFingerTipTransform = b.Transform;
            }
            if (b.Id == OVRSkeleton.BoneId.Hand_ThumbTip) {
                handThumbTipTransform = b.Transform;
            }
        }
        return (handThumbTipTransform.position + handFingerTipTransform.position) / 2f;
    }
}