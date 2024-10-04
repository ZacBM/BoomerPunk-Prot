using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBop : MonoBehaviour
{
    //head bop variables
    public float bobbingSpeed = 12.5f;
    public float bobbingAmount = 0.115f;
    public float idleBobbingSpeed = 4.0f;
    public float idleBobbingAmount = 0.02f;

    public GameObject playerCamera;
    public GameObject cameraRig;

    private float defaultBobbingAmount;
    private float defaultBobbingSpeed;
    private Vector3 originalCameraPosition;

    private void Start()
    {
        originalCameraPosition = cameraRig.transform.localPosition;

        defaultBobbingAmount = bobbingAmount;
        defaultBobbingSpeed = bobbingSpeed;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0;
        float waveslice = Mathf.Sin(Time.time * bobbingSpeed);
        float translateChange = 0.0f;

        if (isMoving)
        {
            //Bobbing Math
            // Increase bobbing when the player moves
            waveslice = Mathf.Sin(Time.time * bobbingSpeed);
            translateChange = waveslice * bobbingAmount;

            float totalAxes = Mathf.Clamp(Mathf.Abs(horizontal) + Mathf.Abs(vertical), 0.0f, 1.0f);
            translateChange *= totalAxes;
        }
        else
        {
            // Subtle bobbing when not moving
            waveslice = Mathf.Sin(Time.time * idleBobbingSpeed);
            translateChange = waveslice * idleBobbingAmount;
        }

        // Actually moves the camera
        cameraRig.transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y + translateChange, originalCameraPosition.z);
    }
}
