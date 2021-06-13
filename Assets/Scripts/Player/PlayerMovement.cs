using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager playerManager;

    // Walking
    [Header("Walking")]
    [SerializeField]
    private float movementSpeed = 5f;
    private float rotationVelocity = 0f;
    [SerializeField]
    private float rotationSmoothTime = 0.1f;

    // Jumping
    [Header("Jumping")]
    [Range(0,99)]
    [SerializeField]
    private float jumpSpeed = 5f;
    [Range(-99,0)]
    [SerializeField]
    private float fallSpeed = -7f;
    [SerializeField]
    private float jumpTime = 1f;
    private float gravity;
    [SerializeField]
    private float groundedDistance = 0.2f;
    private bool grounded;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        gravity = fallSpeed;
    }

    void Update()
    {
        // Walking
        Vector2 inputDirection = playerManager.playerInput.movement.normalized;
        if (inputDirection.magnitude != 0) // 0 means there is no input aka. not moving
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + playerManager.playerCamera.transform.eulerAngles.y;
            float rotationSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.eulerAngles = Vector3.up * rotationSmooth;

            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }

        // Jumping
        float distanceBetweenPlayerAndGround = Mathf.Abs(transform.position.y - playerManager.surroundingCheck.minPosY);
        if (distanceBetweenPlayerAndGround < groundedDistance)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (playerManager.playerInput.jump && grounded)
        {
            StartCoroutine(Jump());
        }

        transform.Translate(Vector3.up * gravity * Time.deltaTime);
        
        // Make sure transform within boundaries
        transform.position = playerManager.surroundingCheck.PositionWithinBoundaries();
    }

    // Increases gravity to jump, then after some time, decrease gravity to fall
    IEnumerator Jump()
    {
        gravity = jumpSpeed;
        yield return new WaitForSeconds(jumpTime);
        gravity = fallSpeed;
    }
}
