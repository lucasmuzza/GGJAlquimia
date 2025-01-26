using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    playerInputHandler = FindFirstObjectByType<PlayerInputHandler>();

    scoreManager = ScoreManager.instance;
    audioManager = AudioManager.instance;

    merchantUIDoc = GetComponent<UIDocument>();
    _rootVisualElement = merchantUIDoc.rootVisualElement;

    firstOfferContainer = _rootVisualElement.Q<VisualElement>("firstOfferHolder");
    secondOfferContainer = _rootVisualElement.Q<VisualElement>("secondOfferHolder");
    thirdOfferContainer = _rootVisualElement.Q<VisualElement>("thirdOfferHolder");
  }

  public void Enable()
  {

  }

  
  public void GenerateOffers()
  {
      Debug.Log("entered");

          _rootVisualElement.style.display = DisplayStyle.Flex;
      for(int i = 0; i < 3; i++)
      {
          int randomIndex = Random.Range(0,ingredientsSO.Count - 1);

          IngredientSO ingredient = ingredientsSO[randomIndex];

          var offer = offerTemplate.Instantiate();

          _offerImage = offer.Q<VisualElement>("offerImage");

          _offerIngredientName = offer.Q<Label>("offerIngredientName");
          _offerIngredientDescription = offer.Q<Label>("offerIngredientDescription");
          _offerIngredientPrice = offer.Q<Label>("offerIngredientPrice");
          _buyButton = offer.Q<Button>("buyButton");
          _buyButton.clicked += BuyItem;

          _offerIngredientName.text = ingredient.ingredientName;
          _offerIngredientDescription.text = ingredient.ingredientDescription;
          _offerIngredientPrice.text = $"Preço: {DetermineIngredientPrice(ingredient.ingredientRarity).ToString()}";

          if(i == 0) firstOfferContainer.Add(offer);

          if(i == 1) secondOfferContainer.Add(offer);

          if(i == 2) thirdOfferContainer.Add(offer);
      }

      
  }

  public int DetermineIngredientPrice(IngredientRarity ingredientRarity)
  {
      if(ingredientRarity == IngredientRarity.Common)
      {
        ingredientPrice = levelOnePrice;
      }
     
      else if(ingredientRarity == IngredientRarity.Rare)
      {
        ingredientPrice = levelThreePrice;
      }

      return (int)ingredientPrice;
  }

  public void BuyItem()
  {
    scoreManager.score -= ingredientPrice;

    _rootVisualElement.style.display = DisplayStyle.None;
    audioManager.PlaySound("Money");
  }

}
