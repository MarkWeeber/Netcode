using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rigidBody;
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turnRate = 30f;

    private Vector2 previousMovementInput;
    private float zRotation;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        if (inputReader != null)
        {
            inputReader.MovementEvent.AddListener(HandleMovement);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        if (inputReader != null)
        {
            inputReader.MovementEvent.RemoveListener(HandleMovement);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            HandleRotation();
        }
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            rigidBody.velocity = new Vector2(bodyTransform.up.x, bodyTransform.up.y)
                * previousMovementInput.y * movementSpeed * Time.fixedDeltaTime;
        }
    }

    private void HandleRotation()
    {
        zRotation = previousMovementInput.x * -turnRate * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void HandleMovement(Vector2 moveDirectionInput)
    {
        previousMovementInput = moveDirectionInput;
    }
}
