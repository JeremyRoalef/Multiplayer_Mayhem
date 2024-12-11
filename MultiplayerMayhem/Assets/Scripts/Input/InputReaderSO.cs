using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Need this for IPlayerActions
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "ScriptableObjects/InputReaderSO")]
//IPlayerActions comes from the controls input system created in unity Input folder
public class InputReaderSO : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;

    //Input System
    private Controls controls;

    void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    //Create this with IPlayerActions. This comes from 
    public void OnMove(InputAction.CallbackContext context)
    {
        //Tell any listeners the movement value of the player
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    //Create this with IPlayerActions
    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        //Tell the listeners if the fire button is pressed
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
    }
}

