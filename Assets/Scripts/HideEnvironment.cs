using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnvironment : MonoBehaviour
{
    // Start is called before the first frame update
    // RGBA(0.192, 0.302, 0.475, 0.020)
    public Color originalColor = new Color(0.192f, 0.302f, 0.475f, 0.020f);
    public void ShowPassthrough() {
        OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
        centerCamera.clearFlags = CameraClearFlags.SolidColor;
        centerCamera.backgroundColor = Color.clear;
        gameObject.SetActive(false);
    }

    public void HidePassthrough() {
        OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
        centerCamera.clearFlags = CameraClearFlags.Skybox;
        centerCamera.backgroundColor = originalColor;
        gameObject.SetActive(true);
    }
}
