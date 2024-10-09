using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
    public Transform cam;

    [SerializeField] private float recoilX = 2.0f;   
    [SerializeField] private float recoilY = 1.0f;   
    [SerializeField] private float recoilZ = 0.5f;  
    [SerializeField] private float kickBackz = 0.2f;

    public float snappiness = 10.0f;
    public float returnAmount = 6.0f;

    void Start()
    {
        initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        // interpolate gun rotation speed
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRotation);

        // interpolate camera rotation speed
        //cam.localRotation = Quaternion.Slerp(cam.localRotation, Quaternion.Euler(currentRotation), Time.deltaTime * snappiness);


        kickBack();
    }

    public void recoil()
    {
        // basically adds the recoil values to the target position
        targetPosition -= new Vector3(0, 0, kickBackz);

        Vector3 cameraRecoil = new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        cam.localRotation = Quaternion.Euler(cam.localRotation.eulerAngles + cameraRecoil);

        targetRotation += new Vector3(recoilX * 0.5f, Random.Range(-recoilY * 0.5f, recoilY * 0.5f), Random.Range(-recoilZ * 0.5f, recoilZ * 0.5f));
        //Debug.Log("REC");
    }

    void kickBack()
    {
        // interpolate gun position speed
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = currentPosition;
    }
}
