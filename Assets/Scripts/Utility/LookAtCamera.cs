using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cam;
    public float offsetTowardsCam = 0f;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        if(!cam)cam = Camera.main.transform;

        Vector3 pos = cam.position;
        Vector3 dir = pos - transform.position;
        if(dir!= Vector3.zero)
        {
            transform.forward = -dir.normalized;
            transform.localPosition = Vector3.zero;
            transform.position += dir.normalized * offsetTowardsCam;
        }
    }
}
