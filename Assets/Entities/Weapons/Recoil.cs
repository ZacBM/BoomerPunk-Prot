using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Vector3 _currentRotation, _targetRotation, _targetPosition, _currentPosition, _initialGunPosition;
    public Transform cam;

    [SerializeField] private float recoilX = 2.0f;   
    [SerializeField] private float recoilY = 1.0f;   
    [SerializeField] private float recoilZ = 0.5f;  
    [SerializeField] private float kickBackz = 0.2f;

    public float snappiness = 10.0f;
    public float returnAmount = 6.0f;

    void Start()
    {
        _initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        // interpolate gun rotation speed
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(_currentRotation);

        // interpolate camera rotation speed
        //cam.localRotation = Quaternion.Slerp(cam.localRotation, Quaternion.Euler(currentRotation), Time.deltaTime * snappiness);


        kickBack();
    }

    public void recoil()
    {
        // basically adds the recoil values to the target position
        _targetPosition -= new Vector3(0, 0, kickBackz);

        Vector3 cameraRecoil = new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        cam.localRotation = Quaternion.Euler(cam.localRotation.eulerAngles + cameraRecoil);

        _targetRotation += new Vector3(recoilX * 0.5f, Random.Range(-recoilY * 0.5f, recoilY * 0.5f), Random.Range(-recoilZ * 0.5f, recoilZ * 0.5f));
        //Debug.Log("REC");
    }

    void kickBack()
    {
        // interpolate gun position speed
        _targetPosition = Vector3.Lerp(_targetPosition, _initialGunPosition, Time.deltaTime * returnAmount);
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = _currentPosition;
    }
}