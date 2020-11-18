using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Arrow : NetworkBehaviour
{
    public Vector2 velocity;
    public GameObject localPlayer;
    public bool hitTarget;
    private void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x,
            transform.position.y);
        Vector2 nextPosition = currentPosition + velocity * Time.deltaTime;

        Debug.DrawLine(currentPosition, nextPosition, Color.red);
        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, nextPosition);
        if(hits.Length > 0)
        {
            foreach(RaycastHit2D hit in hits)
            {
                GameObject other = hit.collider.gameObject;
                if (other != localPlayer)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        print(hit.collider);
                        print("TransformPos" +transform.position);
                        print("currentPos" + currentPosition);
                        print("nextPos" + nextPosition);

                        //gameObject.SetActive(false);
                        //NetworkServer.Destroy(gameObject);
                        hitTarget = true;
                        break;
                    }
                }
            }
        }
        if (!hitTarget)
            transform.position = nextPosition;
        else
            TellServerToDestroyObject(gameObject);

    }

    [Client]
    public void TellServerToDestroyObject(GameObject obj)
    {
        CmdDestroyObject(obj);
    }

    [Command(ignoreAuthority =true)]
    private void CmdDestroyObject(GameObject obj)
    {
        // It is very unlikely but due to the network delay
        // possisble that the other player also tries to
        // destroy exactly the same object beofre the server
        // can tell him that this object was already destroyed.
        // So in that case just do nothing.
        if (!obj) return;

        NetworkServer.Destroy(obj);
    }
}
