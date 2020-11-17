using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Mirror;

public class Controls : NetworkBehaviour
{
    [Space]
    [Header("References:")]
    public GameObject crossHair;
    public Rigidbody2D rb;

    [Space]
    [Header("Prefabs:")]
    public GameObject arrowPrefab;

    [Space]
    [Header("Character attributes:")]
    public float MOVEMENT_BASE_SPEED = 10.0f;
    public float ARROW_BASE_SPEED = 20.0f;

    [Space]
    [Header("Character statistics:")]
    public Vector3 aim;
    public Vector2 movementDirection;
    public bool shoot;

    [Space]
    [Header("Input:")]
    public Vector3 mousePosInWorld;

    public override void OnStartLocalPlayer()
    {
        CameraFollow cameraFollowScript =
            Camera.main.GetComponent<CameraFollow>();
        cameraFollowScript.target = transform; //Fix camera on "me"
        cameraFollowScript.enabled = true;
        crossHair.SetActive(true);
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        ProcessInput();
        Aim();
        Shoot();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        Move();
    }

    void ProcessInput()
    {
        MovementInput();
        AimInput();

        shoot = Input.GetButtonUp("Fire1");
    }

    void MovementInput()
    {
        movementDirection = new Vector3(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"), 0.0f);
        movementDirection.Normalize();
    }

    void AimInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        aim = mousePosInWorld - transform.position;
        aim.Normalize();
    }

    void Move()
    {
        rb.velocity = movementDirection * MOVEMENT_BASE_SPEED;
    }

    void Aim()
    {
        crossHair.transform.position = mousePosInWorld;
    }

    [Command]
    void CmdShootArrow(GameObject arrow)
    {
        if (!isServer) return;
        NetworkServer.Spawn(arrow);
        NetworkServer.Destroy(arrow);

        StartCoroutine(DestroyArrow(arrow));
    }

    IEnumerator DestroyArrow(GameObject arrow)
    {
        yield return new WaitForSeconds(2.0f);
        NetworkServer.Destroy(arrow);

    }

    void Shoot()
    {

        if (!shoot) return;
        Vector2 shootingDirection = new Vector2(aim.x, aim.y);
        GameObject arrow =
            Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        shootingDirection.Normalize();
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(
            shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        arrowScript.velocity = shootingDirection * ARROW_BASE_SPEED;
        arrowScript.localPlayer = gameObject;
        //CmdShootArrow(arrow);
        Destroy(arrow, 2.0f);
    }
}