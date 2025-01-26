using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class Cauldron : MonoBehaviour
{
    public PotionRecipeSO potionRecipe; // this is the current recipe that is given by the order manager
    public UIDocument cauldronUIDoc;
    public VisualTreeAsset ingredientIconTemplate;
    public List<IngredientSO> ingredientsToMix = new List<IngredientSO>();

    private VisualElement _rootVisualElement;
    private VisualElement _ingredientsIconsContainer;

    private bool _potionReady;

    public UnityEvent<int> OnPotionMatched = new UnityEvent<int>();
    public UnityEvent OnPotionUnmatched = new UnityEvent();

    void Start()
    {
        cauldronUIDoc = GetComponent<UIDocument>();
        _rootVisualElement = cauldronUIDoc.rootVisualElement;

        _ingredientsIconsContainer = _rootVisualElement.Q<VisualElement>("ingredientsIconsContainer");
    }

    public void AddIngredient(IngredientSO ingredient)
    {
        ingredientsToMix.Add(ingredient);
        AddIngredientsIcon(ingredient);
        CheckPotionReady();
    }

    public void AddIngredientsIcon(IngredientSO ingredient)
    {   
        var ingredientIcon = ingredientIconTemplate.Instantiate();

        // Query the ingredientIcon from the template
        VisualElement icon = ingredientIcon.Q<VisualElement>("ingredientIcon");

        if (icon != null)
        {
            // Set the icon background image based on the ingredient's icon.
            icon.style.backgroundImage = new StyleBackground(ingredient.ingredientIcon);
            
            // Add the icon to the container
            _ingredientsIconsContainer.Add(ingredientIcon);
        }
        else
        {
            Debug.LogError("ingredientIcon template could not be found.");
        }
    }
    public void ClearIngredientsIcon()
    {
        _ingredientsIconsContainer.Clear();
    }

    public void CheckPotionReady()
    {
        if(ingredientsToMix.Count < potionRecipe.ingredients.Count) return;

        // Check if all the ingredients match the potion's required ingredients.
        else if (ingredientsToMix.Count >= potionRecipe.ingredients.Count)
        {
            bool allIngredientsMatch = true;

            foreach (IngredientSO ingredient in potionRecipe.ingredients)
            {
                if (!ingredientsToMix.Contains(ingredient))
                {
                    allIngredientsMatch = false;

                    Debug.Log("One of the ingredients didn't match. Failed to create the desired potion");
                    ingredientsToMix.Clear();
                    ClearIngredientsIcon();

                    OnPotionUnmatched?.Invoke(); // Event for telling the game that the potion didn't match the required potion
                    break;
                }
            }

            if (allIngredientsMatch)
            {
                _potionReady = true;
                Debug.Log("Potion is ready!");
                OnPotionMatched?.Invoke(potionRecipe.recipeRarityLevel);
            }
        }
    }
}
