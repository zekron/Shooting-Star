using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Object/Player Input SO")]
public class PlayerInputSO :
    ScriptableObject,
    PlayerInputActions.IGameplayActions,
    PlayerInputActions.IPauseMenuActions,
    PlayerInputActions.IGameOverScreenActions,
    PlayerInputActions.IDebugModeActions
{
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction eventOnStopMove = delegate { };
    public event UnityAction eventOnFire = delegate { };
    public event UnityAction eventOnStopFire = delegate { };
    public event UnityAction eventOnDodge = delegate { };
    public event UnityAction eventOnOverdrive = delegate { };
    public event UnityAction eventOnPause = delegate { };
    public event UnityAction eventOnUnpause = delegate { };
    public event UnityAction eventOnLaunchMissile = delegate { };
    public event UnityAction eventOnConfirmGameOver = delegate { };
    public event UnityAction eventOnSetDebugMode = delegate { };
    public event UnityAction eventOnNeedSpawnEnemy = delegate { };
    public event UnityAction eventOnSpawnEnemyNow = delegate { };
    public event UnityAction eventOnSpawnBossNow = delegate { };
    public event UnityAction eventOnSetPlayerInvincible = delegate { };
    public event UnityAction eventOnSetInfiniteEnergy = delegate { };
    public event UnityAction eventOnSetInfiniteMissile = delegate { };

    PlayerInputActions inputActions;

    void OnEnable()
    {
        inputActions = new PlayerInputActions();

        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
        inputActions.DebugMode.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

#if DEBUG_MODE
        inputActions.DebugMode.Enable();
        isUIInput = true;
#endif

        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInputs() => inputActions.Disable();

    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);

    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);

    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            eventOnStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnFire.Invoke();
        }

        if (context.canceled)
        {
            eventOnStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnConfirmGameOver.Invoke();
        }
    }

    public void OnOverDrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnOverdrive.Invoke();
        }
    }

    public void OnSetDebugMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnSetDebugMode.Invoke();
        }
    }

    public void OnNeedSpawnEnemy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnNeedSpawnEnemy.Invoke();
        }
    }

    public void OnSpawnEnemyNow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnSpawnEnemyNow.Invoke();
        }
    }

    public void OnSpawnBossNow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnSpawnBossNow.Invoke();
        }
    }

    public void OnSetPlayerInvincible(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnSetPlayerInvincible.Invoke();
        }
    }

    public void OnSetInfiniteEnergy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnSetInfiniteEnergy.Invoke();
        }
    }

    public void OnSetInfiniteMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventOnSetInfiniteMissile.Invoke();
        }
    }
}