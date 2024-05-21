using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FingerMenuHandler : MonoBehaviour
{
    public GameObject detail;
    public GameObject videoPlayer;
    public Transform cameraTransform;
    public Transform handTransform;
    public float detailActivationDistance = 0.4f;
    public Animator animator;

    private bool isDetailVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        detail.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToCamera = (cameraTransform.position - handTransform.position).normalized;
        float dotProduct = Mathf.Abs(Vector3.Dot(handTransform.forward, directionToCamera));
        

        float distance = Vector3.Distance(detail.transform.position, cameraTransform.position);
        if (distance < detailActivationDistance)
        {
            Debug.Log("Showing text......");
            if (!isDetailVisible)
            {
                isDetailVisible = true;
                animator.SetBool("showText", true);
            }
            Debug.Log(dotProduct.ToString());
            if (dotProduct < 0.4f)
            {
                animator.SetBool("showVideo", true);
            } else
            {
                animator.SetBool("showVideo", false);
            }
          //  StartCoroutine(ShowVideoAfterDelay(3f)); // Start coroutine to show video after 3 second
            
        }
        else if (distance >= detailActivationDistance) {
            isDetailVisible = false;
            animator.SetBool("showText", false);
            //animator.SetBool("showVideo", false);
            // animator.Rebind();
        }
    }

    IEnumerator ShowVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("showVideo", true);
    }


}
