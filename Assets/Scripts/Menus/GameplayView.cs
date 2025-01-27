using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayView : MonoBehaviour
{
    public ScoreManager scoreManagerInstance;
    public IngredientStock stockManager;
    public List<IngredientSO> ingredientsTypes;
    public UIDocument gameplayUIDoc;
    public VisualTreeAsset igredientStockTemplate;

    private VisualElement _rootVisualElement;
    private VisualElement _ingredientStockContainer;

    private VisualElement _tooltip;
    private Label _moneyLabel;

    private void Start()
    {
        scoreManagerInstance = ScoreManager.instance;
        stockManager = IngredientStock.instance;

        gameplayUIDoc = GetComponent<UIDocument>();

        _rootVisualElement = gameplayUIDoc.rootVisualElement;

        DisplayStock();
    }

    private void Update()
    {
        if (GameManager.instance.isGamePaused)
        {
            _rootVisualElement.pickingMode = PickingMode.Ignore;
        }
        else if (!GameManager.instance.isGamePaused)
        {
            _rootVisualElement.pickingMode = PickingMode.Position;
        }

        _moneyLabel = _rootVisualElement.Q<Label>("moneyLabel");
        _moneyLabel.text = $"Moedas: {scoreManagerInstance.GetScore()}";
    }

    private void DisplayStock()
    {
        _ingredientStockContainer = _rootVisualElement.Q<VisualElement>("ingredientStockContainer");
        _ingredientStockContainer.Clear();

        foreach (var ingredientType in ingredientsTypes)
        {
            // Instantiate the template and set up the ingredient
            var stockedIngredient = igredientStockTemplate.Instantiate();
            VisualElement ingredientImage = stockedIngredient.Q<VisualElement>("ingredientImage");
            Label ingredientCountLabel = stockedIngredient.Q<Label>("ingredientAmountLabel");

            VisualElement tooltip = stockedIngredient.Q<VisualElement>("tooltip");
            Label ingredientNameLabel = tooltip.Q<Label>("ingredientNameLabel");
            
            // Set ingredient image and count
            ingredientImage.style.backgroundImage = new StyleBackground(ingredientType.ingredientIcon);
            int count = stockManager.currentStock.FindAll(x => x == ingredientType).Count;
            ingredientCountLabel.text = count.ToString();


            stockedIngredient.RegisterCallback<MouseEnterEvent>((type) => {
                tooltip.style.display = DisplayStyle.Flex;
                ingredientNameLabel.text = ingredientType.ingredientName;
            });

            stockedIngredient.RegisterCallback<MouseLeaveEvent>((type) =>{
                tooltip.style.display = DisplayStyle.None;
            })
            ;

            _ingredientStockContainer.Add(stockedIngredient);
        }
    }

    // Function to add an ingredient to the stock display
    public void AddIngredientToDisplay(IngredientSO ingredient)
    {
        var ingredientElement = FindIngredientElement(ingredient);

        if (ingredientElement != null)
        {
            var countLabel = ingredientElement.Q<Label>("ingredientAmountLabel");
            int count = int.Parse(countLabel.text);
            count++;
            countLabel.text = count.ToString();
        }
        else
        {
            // If the ingredient isn't already displayed, add it
            var stockedIngredient = igredientStockTemplate.Instantiate();

            VisualElement ingredientImage = stockedIngredient.Q<VisualElement>("ingredientImage");
            Label ingredientCountLabel = stockedIngredient.Q<Label>("ingredientAmountLabel");

            ingredientImage.style.backgroundImage = new StyleBackground(ingredient.ingredientIcon);
            ingredientCountLabel.text = "1";

            _ingredientStockContainer.Add(stockedIngredient);
        }
    }

    // Function to remove an ingredient from the stock display
    public void RemoveIngredientFromDisplay(IngredientSO ingredient)
    {
        var ingredientElement = FindIngredientElement(ingredient);

        if (ingredientElement != null)
        {
            var countLabel = ingredientElement.Q<Label>("ingredientAmountLabel");
            int count = int.Parse(countLabel.text);

            if (count > 0)
            {
                count--;
                countLabel.text = count.ToString();
            }
        }
    }

    // Helper function to find the UI element of a specific ingredient
    private VisualElement FindIngredientElement(IngredientSO ingredient)
    {
        foreach (var child in _ingredientStockContainer.Children())
        {
            var ingredientImage = child.Q<VisualElement>("ingredientImage");

            if (ingredientImage.style.backgroundImage.value.sprite == ingredient.ingredientIcon)
            {
                return child;
            }
        }

        return null;
    }
}
