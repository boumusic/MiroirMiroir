using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private static GameCamera instance;
    public static GameCamera Instance { get { if (!instance) instance = FindObjectOfType<GameCamera>(); return instance; } }

    [Header("Position")]
    [SerializeField] private float posSmoothness = 0.2f;
    private Vector3 currentVel;

    [SerializeField] private float minZ = 7;
    [SerializeField] private float maxZ = 30f;
    [SerializeField] private float maxPlayerDistance = 30f;
    [SerializeField] private float yOffset = 1f;
    [SerializeField] private float speedScroll = 5f;

    private float progress = 0f;

    private Player player => UIManager.instance.player;

    private void Update()
    {
        if(player != null)
        {
            progress -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * speedScroll;
            progress = Mathf.Clamp01(progress);
            float z = Mathf.Lerp(minZ, maxZ, progress);
            Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, -z);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVel, posSmoothness);
        }
    }

}
