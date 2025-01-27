using System.Collections;
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
    public PotionOrderManager potionOrderManager;
    public UIDocument cauldronUIDoc;
    public VisualTreeAsset ingredientIconTemplate;
    public List<IngredientSO> ingredientsToMix = new List<IngredientSO>();
    public Material _cauldronMat;

    private GameManager _gameManager;

    private VisualElement _rootVisualElement;
    private VisualElement _ingredientsIconsContainer;

    private AudioManager _audioManager;
    private AudioClip _cauldronAudioClip;

    private bool _potionReady;

    public UnityEvent<int> OnPotionMatched = new UnityEvent<int>();
    public UnityEvent OnPotionUnmatched = new UnityEvent();

    void Start()
    {
        cauldronUIDoc = GetComponent<UIDocument>();
        _rootVisualElement = cauldronUIDoc.rootVisualElement;

        _gameManager = GameManager.instance;

        _audioManager = AudioManager.instance;

        potionOrderManager = PotionOrderManager.instance;
        potionOrderManager.potionToMatch.AddListener(SetPotionRecipe);

        AudioSource cauldronAudio = this.gameObject.AddComponent<AudioSource>();
        _cauldronAudioClip = _audioManager.GetSound("Cauldron");

        cauldronAudio.clip = _cauldronAudioClip;
        cauldronAudio.loop = true;
        cauldronAudio.playOnAwake = true;
        cauldronAudio.Play();

        _ingredientsIconsContainer = _rootVisualElement.Q<VisualElement>("ingredientsIconsContainer");
    }

    private void SetPotionRecipe(PotionRecipeSO potion)
    {
        potionRecipe = potion;
    }

    public void AddIngredient(IngredientSO ingredient)
    {
        Debug.Log($"The ingredient to add is {ingredient}");

        // Check if the ingredient is part of the current potion recipe
        if (!potionRecipe.ingredients.Contains(ingredient))
        {
            Debug.Log($"Ingredient {ingredient.name} is not part of the recipe.");
            HandleFailedPotion();
            return; 
        }

        // Check for duplicates in the current cauldron (ingredientsToMix list)
        if (ingredientsToMix.Contains(ingredient))
        {
            Debug.Log($"Ingredient {ingredient.name} is already added.");
            HandleFailedPotion();
            return; 
        }

        
        ingredientsToMix.Add(ingredient);
        AddIngredientsIcon(ingredient); 

       
        _cauldronMat.SetFloat("_SplashTime", Time.time);
        _cauldronMat.SetColor("_ColorA", _cauldronMat.GetColor("_ColorB"));
        _cauldronMat.SetColor("_ColorB", new Vector4(Random.value, Random.value, Random.value, 1));

        // Play sound effect for ingredient drop
        _audioManager.PlaySound("CauldronDrop");

        // Check if the potion is ready to be brewed
        CheckPotionReady();
    }

    public void AddIngredientsIcon(IngredientSO ingredient)
    {
        var ingredientIcon = ingredientIconTemplate.Instantiate();

        // Query the ingredientIcon from the template
        VisualElement icon = ingredientIcon.Q<VisualElement>("Image");

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
        // If there aren't enough ingredients to match the recipe, exit early
        if (ingredientsToMix.Count < potionRecipe.ingredients.Count) return;

        // Check if the mixed ingredients exactly match the recipe ingredients
        bool isExactMatch = AreIngredientsExactMatch();

        if (isExactMatch)
        {
            // Potion is successfully created
            _potionReady = true;
            Debug.Log("Potion is ready!");

            _audioManager.PlaySound("PotionRight");

            // Handle potion success (e.g., score, clear ingredients/icons, etc.)
            StartCoroutine(HandleSuccessfulPotion(2));
        }
        else
        {
            // Handle potion failure
            Debug.Log("Potion failed: incorrect ingredients.");
            HandleFailedPotion();
        }
    }

    private bool AreIngredientsExactMatch()
    {
        // Create dictionaries for the recipe ingredients and the mixed ingredients
        Dictionary<IngredientSO, int> recipeIngredientCounts = GetIngredientCounts(potionRecipe.ingredients);
        Dictionary<IngredientSO, int> mixedIngredientCounts = GetIngredientCounts(ingredientsToMix);

        // Compare the two dictionaries
        foreach (var pair in recipeIngredientCounts)
        {
            // Check if the mixed ingredients contain the recipe ingredient with the exact count
            if (!mixedIngredientCounts.TryGetValue(pair.Key, out int count) || count != pair.Value)
            {
                return false; // Mismatch found
            }
        }

        // Ensure no extra ingredients in the mixed ingredients
        return recipeIngredientCounts.Count == mixedIngredientCounts.Count;
    }

    private Dictionary<IngredientSO, int> GetIngredientCounts(List<IngredientSO> ingredients)
    {
        Dictionary<IngredientSO, int> ingredientCounts = new Dictionary<IngredientSO, int>();

        foreach (var ingredient in ingredients)
        {
            if (ingredientCounts.ContainsKey(ingredient))
            {
                ingredientCounts[ingredient]++;
            }
            else
            {
                ingredientCounts[ingredient] = 1;
            }
        }

        return ingredientCounts;
    }

    private void HandleFailedPotion()
    {
        // Clear the ingredients list and icons
        ingredientsToMix.Clear();
        ClearIngredientsIcon();



        potionOrderManager.OrderFailed();

        // _gameManager.orderFailedAmount++;

        OnPotionUnmatched?.Invoke();
    }

    private IEnumerator HandleSuccessfulPotion(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Clear ingredients and icons after the wait time
        ClearIngredientsIcon();
        ingredientsToMix.Clear();

        // Add score based on the recipe rarity level
        ScoreManager.instance.AddScore(potionRecipe.recipeRarityLevel);

        potionOrderManager.OrderCompleted();
        // Reset potion ready flag
        _potionReady = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bubble"))
        {
            _audioManager.PlaySound("CauldronDrop");
            IngredientSO ingredient = other.gameObject.GetComponent<Ingredient>().ingredient;
            AddIngredient(ingredient);
            Destroy(other.gameObject);
        }
    }
}
