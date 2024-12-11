using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//Not MonoBehavior, but NetworkBehavior
public class PlayerMover : NetworkBehaviour
{
    [Header("References")]

    [SerializeField]
    InputReaderSO inputReaderSO;

    [SerializeField]
    Transform bodyTransform;

    [SerializeField]
    Rigidbody2D playerRb;


    [Header("Settings")]

    [SerializeField]
    float fltMoveSpeed = 4f;

    [SerializeField]
    float fltTurningRate = 30f;


    Vector2 previousMovementInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }

        //Subscribe to the move event
        inputReaderSO.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }

        //Unsubscribe from the move event
        inputReaderSO.MoveEvent -= HandleMove;
    }

    void Update()
    {
        if (!IsOwner) { return; }
        //transform.Translate(previousMovementInput);
        //playerRb.velocity = previousMovementInput * fltMoveSpeed;

        float zRotation = previousMovementInput.x * -fltTurningRate * Time.deltaTime;
        bodyTransform.Rotate(0,0,zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return; }

        //In 2D, forward is up
        playerRb.velocity = (Vector2) bodyTransform.up * previousMovementInput.y * fltMoveSpeed;

    }

    void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }
}
