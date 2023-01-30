using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera topCamera;


    // Update is called once per frame
    void Update()
    {
        ChangeCamera();
    }

    private void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            if (mainCamera.gameObject.activeSelf)
            {
                mainCamera.gameObject.SetActive(false);
                topCamera.gameObject.SetActive(true);
            }
            else
            {
                topCamera.gameObject.SetActive(false);
                mainCamera.gameObject.SetActive(true);
            }
        }
    }
}
