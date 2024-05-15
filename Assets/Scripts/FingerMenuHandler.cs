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
        float distance = Vector3.Distance(detail.transform.position, cameraTransform.position);
        if (distance < detailActivationDistance && !isDetailVisible)
        {
            animator.SetBool("showText", true);
            isDetailVisible = true;
        }
        else if (distance >= detailActivationDistance && isDetailVisible) {
            isDetailVisible = false;
            animator.SetBool("showText", false);
        }
    }


}
