using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager playerManager;

    [SerializeField]
    private float movementSpeed = 5f;
    private float yVelocity = 0f;
    private float smoothTime = 0.1f;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        Vector2 inputDirection = playerManager.playerInput.movement.normalized;

        if (inputDirection.magnitude != 0)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + playerManager.playerCamera.transform.eulerAngles.y;
            float rotationSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref yVelocity, smoothTime);
            transform.eulerAngles = Vector3.up * rotationSmooth;

            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }

        transform.position = playerManager.surroundingCheck.PositionWithinBoundaries();
    }
}
