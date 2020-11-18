using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Square : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }
}
