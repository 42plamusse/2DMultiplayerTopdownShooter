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
    public Animator animator;
    public Transform arrowSpawner;

    [Space]
    [Header("Prefabs:")]
    public GameObject arrowPrefab;

    [Space]
    [Header("Character attributes:")]
    public float MOVEMENT_BASE_SPEED = 10.0f;
    public float ARROW_BASE_SPEED = 20.0f;
    public float SHOOTING_RATE = 0.5f;

    [Space]
    [Header("Character statistics:")]
    public float mouvementSpeed = 1.0f;
    public Vector3 aim;
    public Quaternion aimRotation;
    public Vector3 movementDirection;
    public bool shoot;
    public bool aiming;
    public float nextShoot = 0f;


    [Space]
    [Header("Input:")]
    public Vector3 mousePosInWorld;
    private void Start()
    {
        if (isLocalPlayer)
        {
            nextShoot = SHOOTING_RATE;
        }
    }
    void Update()
    {
        if (isLocalPlayer)
        {
            ProcessInput();
            Animate();
            Aim();
            if (shoot)
            {
                CmdShootArrow(aim, arrowSpawner.position, netId);
                shoot = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
            Move();
    }

    void Animate()
    {
        animator.SetBool("Aiming", aiming);
    }

    void ProcessInput()
    {
        MovementInput();
        AimInput();
        if (Input.GetButtonDown("Fire1"))
            aiming = true;
        if (aiming)
        {
            mouvementSpeed = 0.8f;
            if (nextShoot <= 0f)
            {
                shoot = true;
                aiming = false;
                nextShoot = SHOOTING_RATE;
                mouvementSpeed = 1f;
            }
            nextShoot -= Time.deltaTime;
        }
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
        aimRotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg);

    }

    void Move()
    {
        rb.MovePosition(transform.position +
            movementDirection *
            mouvementSpeed *
            MOVEMENT_BASE_SPEED *
            Time.fixedDeltaTime);
    }

    void Aim()
    {
        crossHair.transform.position = mousePosInWorld;
        transform.rotation = aimRotation;
    }

    [Command]
    void CmdShootArrow(Vector2 aim, Vector3 initialPosition, uint _netId)
    {
        Vector2 shootingDirection = new Vector2(aim.x, aim.y);
        Vector3 offset = aim * 1.5f;
        Quaternion arrowTransformRotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        GameObject arrow =
            Instantiate(arrowPrefab,
            initialPosition,
            arrowTransformRotation);
        shootingDirection.Normalize();
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.velocity = shootingDirection * ARROW_BASE_SPEED;
        arrowScript.shooterId = _netId;
        NetworkServer.Spawn(arrow);
        Destroy(arrow, 5.0f);

    }
}