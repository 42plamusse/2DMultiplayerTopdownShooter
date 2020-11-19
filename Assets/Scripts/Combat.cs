using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Combat : NetworkBehaviour
{
    [SyncVar]
    public int health = 100;
    [SyncVar]
    public int death = 0;

    public void TakeDamage(int amount)
    {
        if (!isServer) return;
        health -= amount;
        if (health <= 0)
            Respawn();
    }

    void Respawn()
    {
        death++;
        health = 100;
        Trpc_respawn();
    }

    [TargetRpc]
    void Trpc_respawn()
    {
        transform.position = GetComponent<Player>().initialPosition;

    }
}
