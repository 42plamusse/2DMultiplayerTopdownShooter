using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform target; //what to follow
    public float smoothing = 5f; //camera speed

    //Vector3 offset;

    //void Start()
    //{
    //    offset = transform.position - target.position;
    //    //transform.position = new Vector3(target.position.x,
    //    //    target.position.y, transform.position.z);
    //}

    void Update()
    {
        if (!target) return;
        Vector3 targetCamPos = target.position;
        targetCamPos.z = transform.position.z;
        transform.position = targetCamPos;
        //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}