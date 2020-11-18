using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform target; //what to follow
    public float smoothing = 5f; //camera speed

    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (!target) return;
        Vector3 targetCamPos = target.position + offset;
        targetCamPos.z = transform.position.z;
        transform.position = targetCamPos;
        //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}