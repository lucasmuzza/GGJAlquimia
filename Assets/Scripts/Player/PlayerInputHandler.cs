using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInput playerInput;
    public UnityEvent<bool> onGamePaused = new UnityEvent<bool>();
    public bool hasPaused;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if(context.performed && !hasPaused)
        {
            hasPaused = true;
            onGamePaused?.Invoke(true);
        }
        else if(context.performed && hasPaused)
        {
            hasPaused = false;
            onGamePaused?.Invoke(false);
        }
    }
}
