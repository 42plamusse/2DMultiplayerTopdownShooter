using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public GameObject crossHair;

    public override void OnStartLocalPlayer()
    {
        CameraFollow cameraFollowScript =
            Camera.main.GetComponent<CameraFollow>();
        cameraFollowScript.target = transform; //Fix camera on "me"
        cameraFollowScript.enabled = true;
        crossHair.SetActive(true);
        GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        transform.name = netId.ToString();
    }
}
