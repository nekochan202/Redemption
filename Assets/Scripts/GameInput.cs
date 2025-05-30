using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public bool IsPausePressed()
    {
        return playerInputActions.Player.Pause.WasPressedThisFrame();
    }

    public bool IsMedKitUsed()
    {
        return playerInputActions.Player.UseMedKit.WasPressedThisFrame();
    }

    public bool IsReloadPressed()
    {
        return Input.GetKeyDown(KeyCode.R);
    }
}