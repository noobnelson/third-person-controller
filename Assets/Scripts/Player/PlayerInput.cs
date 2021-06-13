using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    internal Vector2 movement;
    internal Vector2 mouseInput;
    internal bool jump;
    [SerializeField]
    private string jumpKey = "space";

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        jump = Input.GetKeyDown(jumpKey);
    }
}
