using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class PotionOrderManager : MonoBehaviour
{
    public static PotionOrderManager instance;
    public PotionOrderDatabase potionOrderDatabase;
    public VisualTreeAsset orderTemplate;

    public PotionOrder potionOrder;

    private AudioManager _audioManager;

    private GameplayView _gameplayView;

    private UIDocument _gameplayUIDoc;

    private VisualElement _rootVisualElement;
    private VisualElement _orderContainer;
    private VisualElement _orderSpriteImage;

    private Button _dropClientButton;

    private Label _orderNameLabel;
    private Label _orderDescriptionLabel;

    public UnityEvent<PotionRecipeSO> potionToMatch = new UnityEvent<PotionRecipeSO>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        _audioManager = AudioManager.instance;

        _gameplayView = FindFirstObjectByType<GameplayView>();

        _gameplayUIDoc = _gameplayView.gameplayUIDoc;

        _rootVisualElement = _gameplayUIDoc.rootVisualElement; 

        _orderContainer = _rootVisualElement.Q<VisualElement>("OrderContainer"); 

        _dropClientButton = _orderContainer.Q<Button>("DropClientButton");

        _dropClientButton.clicked += OrderFailed;

        GenerateOrder();
    }

    public void GenerateOrder()
    {
        // Check if there is an existing order (other than the "DropClientButton") and remove it
        var existingOrder = _orderContainer.Children().FirstOrDefault(child => child != _dropClientButton);
        if (existingOrder != null)
        {
            existingOrder.RemoveFromHierarchy();
        }

        // Get a random order from the potionOrderDatabase
        List<PotionOrder> potionOrders = new List<PotionOrder>(potionOrderDatabase.potionOrders);
        int randomIndex = Random.Range(0, potionOrders.Count);
        potionOrder = potionOrders[randomIndex];

        // Invoke the UnityEvent with the selected potion recipe
        potionToMatch?.Invoke(potionOrder.potionRecipe);

        // Remove the selected order from the pool (if needed for avoiding repeats)
        potionOrders.Remove(potionOrder);

        // Instantiate the order template
        var order = orderTemplate.Instantiate();

        // Populate the order details
        _orderNameLabel = order.Q<Label>("OrderNameLabel");
        _orderNameLabel.text = potionOrder.potionName;

        _orderDescriptionLabel = order.Q<Label>("OrderDescriptionLabel");
        _orderDescriptionLabel.text = potionOrder.potionDescription;

        // Add the new order to the order container
        _orderContainer.Add(order);
    }


    public void OrderCompleted()
    {
        // Plays the sound of sucess and generate the next order
        _audioManager.PlaySound("PotionSucess");
        GenerateOrder();
    }

    public void OrderFailed()
    {
        // Plays the sound of failure and generate the next order
        _audioManager.PlaySound("PotionFailure");
        GenerateOrder();
    }
}
