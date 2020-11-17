using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Arrow : NetworkBehaviour
{
    public Vector2 velocity;
    public GameObject localPlayer;

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
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
        transform.position = nextPosition;
    }
}
