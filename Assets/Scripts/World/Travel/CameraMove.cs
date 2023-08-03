using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    bool traveling = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onStartTravel(float cameraSpeed) 
    {
        traveling = true;
        StartCoroutine(moveCamera(cameraSpeed));
    }
    private IEnumerator moveCamera(float cameraSpeed)
    {
        while (traveling) 
        {
            transform.Translate(Vector3.right * cameraSpeed, Space.Self);
            yield return null;
        }
    }
}
