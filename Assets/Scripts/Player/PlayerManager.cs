using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    internal PlayerInput playerInput;
    internal PlayerMovement playerMovement;
    internal ThirdPersonCamera playerCamera;
    internal SurroundingCheck surroundingCheck;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        surroundingCheck = GetComponent<SurroundingCheck>();
        playerCamera = FindObjectOfType<ThirdPersonCamera>();    
    }
}
