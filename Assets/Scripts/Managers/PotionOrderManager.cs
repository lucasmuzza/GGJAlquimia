using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class PotionOrderManager : MonoBehaviour
{
    public PotionOrderDatabase potionOrderDatabase;
    public VisualTreeAsset orderTemplate;

    private AudioManager _audioManager;

    private GameplayView _gameplayView;

    private UIDocument _gameplayUIDoc;

    private VisualElement _rootVisualElement;
    private VisualElement _orderContainer;
    private VisualElement _orderSpriteImage;

    private Button _dropClientButton;

    private Label _orderNameLabel;
    private Label _orderDescriptionLabel;

    private void Start()
    {
        _audioManager = AudioManager.instance;

        _gameplayView = FindFirstObjectByType<GameplayView>();

        _gameplayUIDoc = _gameplayView.gameplayUIDoc;

        _rootVisualElement = _gameplayUIDoc.rootVisualElement; 

        _orderContainer = _rootVisualElement.Q<VisualElement>("OrderContainer"); 

        _dropClientButton = _orderContainer.Q<Button>("DropClientButton");

        _dropClientButton.clicked += OrderFailed;
    }

    public void GenerateOrder()
    {
        int randomIndex = Random.Range(0,potionOrderDatabase.potionOrders.Count);
        PotionOrder potionOrder = potionOrderDatabase.potionOrders[randomIndex];

        potionOrderDatabase.potionOrders.Remove(potionOrder); // remove the selected order from the pool of order

        var order = orderTemplate.Instantiate(); // the order template instantiated

        _orderNameLabel = order.Q<Label>("OrderName");
        _orderNameLabel.text = potionOrder.potionName;

        _orderDescriptionLabel = order.Q<Label>("OrderDescription");
        _orderDescriptionLabel.text = potionOrder.potionDescription;

        _orderSpriteImage = order.Q<VisualElement>("OrderSpriteImage");
        _orderSpriteImage.style.backgroundImage = new StyleBackground(potionOrder.potionSprite);

        _orderContainer.Add(order); // add the order to the order container
    }

    public void OrderCompleted()
    {
        // Plays the sound of sucess and generate the next order
        _audioManager.PlaySound("PotionSucess");
    }

    public void OrderFailed()
    {
        // Plays the sound of failure and generate the next order
        _audioManager.PlaySound("PotionFailure");
    }
}
