using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private PlayerInput playerInput;
    private SurroundingCheck surroundingCheck;

    [SerializeField]
    private Transform cameraPivot;
    [SerializeField]
    private float distanceFromPivot = 10f;
    [SerializeField]
    private float mouseSensitivity = 10f;

    private float pitch;
    private float yaw;
    [SerializeField]
    private float pitchMin = -40;
    [SerializeField]
    private float pitchMax = 40;

    private Vector3 rotationSmoothVelocity;
    private float rotationSmoothTime = 0.1f;
    private Vector3 positionSmoothVelocity = Vector3.zero;
    private float positionSmoothTime = 0.01f;

    private Vector3 currentRotation;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        surroundingCheck = GetComponent<SurroundingCheck>();
    }

    void LateUpdate()
    {
        yaw += playerInput.mouseInput.x * mouseSensitivity;
        pitch -= playerInput.mouseInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Vector3 targetPosition = cameraPivot.position - transform.forward * distanceFromPivot;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref positionSmoothVelocity, positionSmoothTime);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = surroundingCheck.PositionWithinBoundaries();
    }
}
