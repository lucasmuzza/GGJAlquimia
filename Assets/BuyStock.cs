using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BuyStock : MonoBehaviour
{
    public static BuyStock instance;
    public List<IngredientSO> ingredientsSO;
    public IngredientStock stockManager;

    public UIDocument merchantUIDoc;
    public VisualTreeAsset offerTemplate;

    private VisualElement _rootVisualElement;

    private VisualElement _offerImage;

    private VisualElement _offersContainer;

    private VisualElement firstOfferContainer;
    private VisualElement secondOfferContainer;
    private VisualElement thirdOfferContainer;

    private Label _offerIngredientName;
    private Label _offerIngredientDescription;
    private Label _offerIngredientPrice;

    private Button _buyButton;

    public PlayerInputHandler playerInputHandler;

    public ScoreManager scoreManager;
    public AudioManager audioManager;
    public float ingredientPrice;

    public float levelOnePrice;
    public float levelTwoPrice;
    public float levelThreePrice;

    private void Awake()
    {
        if (instance == null)
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
        scoreManager = ScoreManager.instance;
        audioManager = AudioManager.instance;

        merchantUIDoc = GetComponent<UIDocument>();
        _rootVisualElement = merchantUIDoc.rootVisualElement;

        _offersContainer = _rootVisualElement.Q<VisualElement>("OffersContainer");

        _offersContainer.style.display = DisplayStyle.None;

        firstOfferContainer = _rootVisualElement.Q<VisualElement>("firstOfferHolder");
        secondOfferContainer = _rootVisualElement.Q<VisualElement>("secondOfferHolder");
        thirdOfferContainer = _rootVisualElement.Q<VisualElement>("thirdOfferHolder");

        Debug.Assert(_rootVisualElement != null, "Root Visual Element is null. Is the UIDocument assigned?");
    }

    public void GenerateOffers()
    {
        Debug.Log("entered");

        _offersContainer.style.display = DisplayStyle.Flex;

        firstOfferContainer.Clear();
        secondOfferContainer.Clear();
        thirdOfferContainer.Clear();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, ingredientsSO.Count - 1);

            IngredientSO ingredient = ingredientsSO[randomIndex];

            var offer = offerTemplate.Instantiate();

            _offerImage = offer.Q<VisualElement>("offerImage");

            _offerImage = offer.Q<VisualElement>("offerImage");
            _offerIngredientName = offer.Q<Label>("offerIngredientName");
            _offerIngredientDescription = offer.Q<Label>("offerIngredientDescription");
            _offerIngredientPrice = offer.Q<Label>("offerIngredientPrice");
            _buyButton = offer.Q<Button>("buyButton");

            // Bind the specific ingredient to the button click
            _buyButton.clicked += () => BuyItem(ingredient);

            _offerImage.style.backgroundImage = new StyleBackground(ingredient.ingredientIcon);
            _offerIngredientName.text = ingredient.ingredientName;
            _offerIngredientDescription.text = ingredient.ingredientDescription;

            int price = DetermineIngredientPrice(ingredient.ingredientRarity);
            Debug.Log(price);
            _offerIngredientPrice.text = $"Preço: {price}";

            if (i == 0) firstOfferContainer.Add(offer);

            if (i == 1) secondOfferContainer.Add(offer);

            if (i == 2) thirdOfferContainer.Add(offer);
        }
    }

    public int DetermineIngredientPrice(IngredientRarity ingredientRarity)
    {
        if (ingredientRarity == IngredientRarity.Common)
        {
            ingredientPrice = levelOnePrice;
        }
        else if (ingredientRarity == IngredientRarity.Rare)
        {
            ingredientPrice = levelThreePrice;
        }

        return (int)ingredientPrice;
    }

    public void BuyItem(IngredientSO ingredient)
    {
        int price = DetermineIngredientPrice(ingredient.ingredientRarity);

        if (scoreManager.score >= price)
        {
            scoreManager.score -= price;

            // Add the purchased ingredient to the stock
            stockManager.AddIngredientToStock(ingredient);

            audioManager.PlaySound("Money");

            Debug.Log($"Bought {ingredient.ingredientName} for {price}");
        }
        else
        {
            Debug.Log("Not enough score to buy this ingredient.");
        }

        // Hide offers container after purchase
        _offersContainer.style.display = DisplayStyle.None;

        GameManager.instance.isGamePaused = false;
    }
}
