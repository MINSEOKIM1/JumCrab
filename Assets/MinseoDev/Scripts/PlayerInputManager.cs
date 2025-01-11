using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 move;
    public bool jump;
    public bool pause;

    public void OnMove(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            jump = true;
        }

        if (value.canceled)
        {
            jump = false;
        }
    }

    public void Pause()
    {
        pause = true;
    }
    
    public void OnPause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            pause = true;
        }
    }
}
