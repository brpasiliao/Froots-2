using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInputActions inputActions;
    public static event Action<InputActionMap> actionMapchange;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

    }

    private void Start()
    { 
        //start with the player controller enabled.
        ToggleActionMap(inputActions.Player);
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
        {
            return;
        }

        inputActions.Disable();
        actionMapchange?.Invoke(actionMap);
        actionMap.Enable();
    }

    public static void ChangeGrassoControls(InputActionMap actionmap)
    {
        if (actionmap.enabled)
        {
            return;
        }

        if(actionmap.name == "GrassoAimV1")
        {
            inputActions.GrassoAimV1.Enable();
            inputActions.GrassoAimV2.Disable();
            inputActions.GrassoAimV3.Disable();
        }
        else if(actionmap.name == "GrassoAimV2")
        {
            inputActions.GrassoAimV1.Disable();
            inputActions.GrassoAimV2.Enable();
            inputActions.GrassoAimV3.Disable();
        }
        else if(actionmap.name == "GrassoAimV3")
        {
            inputActions.GrassoAimV1.Disable();
            inputActions.GrassoAimV2.Disable();
            inputActions.GrassoAimV3.Enable();
        }
    }
}
