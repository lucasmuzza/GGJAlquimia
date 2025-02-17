using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInput playerInput;
    public BuyStock buyStock;
    public GameManager gameManager;
    public UnityEvent<bool> onGamePaused = new UnityEvent<bool>();
    public UnityEvent<string> onChangedSceneView = new UnityEvent<string>();
    public UnityEvent onMerchantCalled = new UnityEvent();
    public bool hasPaused;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        gameManager = GameManager.instance;
        buyStock = BuyStock.instance;
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

    public void ChangeViewInput(InputAction.CallbackContext context)
    {
        if(context.performed && context.action.name == "ChangeToPotionView")
        {
            onChangedSceneView?.Invoke("PotionView");
        }
        else if(context.performed && context.action.name == "ChangeToClientView")
        {
            onChangedSceneView?.Invoke("ClientView");
        }
    }

    public void MerchantInput(InputAction.CallbackContext context)
    {
        if(context.performed && !gameManager.isGamePaused)
        {
            Debug.Log("Key pressed");
            buyStock.GenerateOffers();
            gameManager.isGamePaused = true;
        }

        if(context.performed && gameManager.isGamePaused)
        {
            gameManager.isGamePaused = false;
        }
    }
}
