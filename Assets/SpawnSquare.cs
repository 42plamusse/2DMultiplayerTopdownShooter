using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnSquare : NetworkBehaviour
{
    public GameObject squarePrefab;
    private void Update()
    {
        //if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.X))
            CmdDropSquare();
    }

    [Command(ignoreAuthority =true)]
    void CmdDropSquare()
    {
        if (squarePrefab != null)
        {
            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = transform.rotation;
            GameObject cube = Instantiate(squarePrefab, spawnPos, spawnRot);
            NetworkServer.Spawn(cube);
        }
    }
}

