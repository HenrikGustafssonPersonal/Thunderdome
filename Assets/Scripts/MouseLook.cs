using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;
    public Transform camHolder;
    public GameObject cameraEffects;


    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;   
    }

    // Update is called once per frame
    void Update()
    {

    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f,90f);


    transform.localRotation = Quaternion.Euler(xRotation,0f, 0f);
    camHolder.Rotate(Vector3.up * mouseX);


    }
    //Dash functions return to original state instantly after adjustment
    public void DoDashFov(float startValue,float endValue, float inTime, float outTime) 
    {
        GetComponent<Camera>().DOFieldOfView(endValue, inTime).OnComplete(() => {
            GetComponent<Camera>().DOFieldOfView(startValue, outTime);
        });
     }
    public void DoDashTilt(float zTilt, float inTime, float outTime) {
        cameraEffects.transform.DOLocalRotate(new Vector3(0, 0, zTilt), inTime).OnComplete(()=> { 
            cameraEffects.transform.DOLocalRotate(new Vector3(0, 0, 0), outTime); });
    }

    public void DoFov(float endValue, float time)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, time);
    }
}
