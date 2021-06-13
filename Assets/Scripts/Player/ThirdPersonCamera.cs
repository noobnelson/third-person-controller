using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private PlayerInput playerInput;
    private SurroundingCheck surroundingCheck;
    
    [Header("Position")]
    [SerializeField]
    private Transform cameraPivot; // the point where the camera rotates around
    [SerializeField]
    private float distanceFromPivot = 10f;
    [SerializeField]
    private float mouseSensitivity = 10f;

    private Vector3 positionSmoothVelocity = Vector3.zero;
    [SerializeField]
    private float positionSmoothTime = 0.01f;

    [Header("Rotation")]
    [Range(-90, 0)]
    [SerializeField]
    private float pitchMin = -40;
    [Range(0, 90)]
    [SerializeField]
    private float pitchMax = 40;
    private float pitch; // x rotation 
    private float yaw; // y rotation

    private Vector3 rotationSmoothVelocity;
    [SerializeField]
    private float rotationSmoothTime = 0.1f;

    private Vector3 currentRotation;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        surroundingCheck = GetComponent<SurroundingCheck>();
    }

    void LateUpdate()
    {
        // Rotation
        yaw += playerInput.mouseInput.x * mouseSensitivity; // moving mouse horizontally will rotate the y axis. imagine a plane going in a circle.
        pitch -= playerInput.mouseInput.y * mouseSensitivity; // moving mouse vertically will rotate x axis. imagine a plane doing front flips or back flips OR 'pitch'-ing a ball
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        // Position
        Vector3 targetPosition = cameraPivot.position - transform.forward * distanceFromPivot;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref positionSmoothVelocity, positionSmoothTime);

        transform.position = surroundingCheck.PositionWithinBoundaries();
    }
}
