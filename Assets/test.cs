using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject lookAt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(lookAt.transform.position);
        Vector3 angles = transform.rotation.eulerAngles;
        angles.x -= 90;
        transform.rotation = Quaternion.Euler(angles);
    }
}
