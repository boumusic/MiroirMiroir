using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovingCube : NetworkBehaviour
{
    public float amplitude;
    public float speed;

    private float start;

    private void Start()
    {
        start = Random.Range(-5f, 5f);
    }

    void Update()
    {
        if(hasAuthority)
        {
            Debug.Log(Time.frameCount);
            float y = Mathf.Sin(Time.frameCount * speed + start) * amplitude;
            Debug.Log(y);
            transform.position = new Vector3(start, y, 0);

        }
        
    }
}
