using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedRaycastHit
{
    public Vector3 normal;
    public Vector3 point;
    public GameObject gameObject;

    public SerializedRaycastHit(RaycastHit hit)
    {
        normal = hit.normal;
        point = hit.point;
        gameObject = hit.collider.gameObject;
    }
}

public class RaycastHitSerializer : MonoBehaviour
{
    public List<SerializedRaycastHit> hits = new List<SerializedRaycastHit>();

    public void UpdateHits(RaycastHit[] raycastHits)
    {
        hits.Clear();
        for (int i = 0; i < raycastHits.Length; i++)
        {
            hits.Add(new SerializedRaycastHit(raycastHits[i]));
        }
    }
}
