using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Arrow : NetworkBehaviour
{
    [Header("Server")]
    public Vector2 velocity;
    public uint shooterId;

    [Header("Synced")]
    [SyncVar]
    public bool hasHit = false;
    [SyncVar]
    public Transform parent;

    private bool nested = false;

    private void FixedUpdate()
    {
        if (hasHit)
        {
            if (!nested)
            {
                var arrowNest = new GameObject();
                arrowNest.transform.parent = parent;
                arrowNest.name = "NestingArrow";
                transform.parent = arrowNest.transform;
                nested = true;
             }
        }
        if (!isServer || hasHit) return;
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
                if (other.name != shooterId.ToString())
                {
                    if (other.CompareTag("Player"))
                    {
                        hasHit = true;
                        parent = hit.collider.transform;
                        other.GetComponent<Combat>().TakeDamage(10);
                        break;
                    }
                    else if (other.CompareTag("Walls"))
                    {
                        hasHit = true;
                        parent = hit.collider.transform;
                        break;
                    }
                }
            }
        }
        transform.position = nextPosition;
    }

    private void OnDestroy()
    {
        if (transform.parent && transform.parent.name == "NestingArrow")
            Destroy(transform.parent.gameObject);
    }
}
