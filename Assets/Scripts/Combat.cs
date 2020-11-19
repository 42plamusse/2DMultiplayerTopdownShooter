using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Combat : NetworkBehaviour
{
    [SyncVar]
    public int health = 100;

    public void TakeDamage(int amount)
    {
        if (!isServer) return;
        health -= amount;
        if (health <= 0)
            Respawn();
    }

    void Respawn()
    {
        health = 100;
        transform.position = Vector3.zero;
    }
}
