using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.VFX;

[RequireComponent(typeof(BoxCollider2D))]
public class Cauldron : MonoBehaviour
{
    public PotionRecipeSO potionRecipe; // this is the current recipe that is given by the order manager
    public PotionOrderManager potionOrderManager;
    public UIDocument cauldronUIDoc;
    public VisualTreeAsset ingredientIconTemplate;
    public List<IngredientSO> ingredientsToMix = new List<IngredientSO>();
    public Material _cauldronMat;
    public VisualEffect _cauldronEffect;

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

        _audioManager = AudioManager.instance;

        potionOrderManager = PotionOrderManager.instance;
        potionOrderManager.potionToMatch.AddListener(SetPotionRecipe);

        AudioSource cauldronAudio = this.gameObject.AddComponent<AudioSource>();
        _cauldronAudioClip = _audioManager.GetSound("Cauldron");
        
        cauldronAudio.clip = _cauldronAudioClip;
        cauldronAudio.loop = true;
        cauldronAudio.playOnAwake = true;
        cauldronAudio.Play();

        _cauldronEffect?.SetVector4(("Color"), _cauldronMat.GetColor("_ColorA"));

        _ingredientsIconsContainer = _rootVisualElement.Q<VisualElement>("ingredientsIconsContainer");
    }

    private void SetPotionRecipe(PotionRecipeSO potion)
    {
        potionRecipe = potion;
    }

    public void AddIngredient(IngredientSO ingredient)
    {
        Debug.Log($"The ingredient to add is {ingredient}");

        ingredientsToMix.Add(ingredient);
        AddIngredientsIcon(ingredient);

        _cauldronMat.SetFloat("_SplashTime",Time.time);
        _cauldronMat.SetColor("_ColorA",_cauldronMat.GetColor("_ColorB"));
        _cauldronMat.SetColor("_ColorB", new Vector4(Random.value,Random.value,Random.value,1));
        _cauldronEffect.SetVector4(("Color"), _cauldronMat.GetColor("_ColorA"));

        _audioManager.PlaySound("CauldronDrop");
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
        // Create a copy of the recipe ingredients to check against
        List<IngredientSO> recipeIngredients = new List<IngredientSO>(potionRecipe.ingredients);

        // Iterate through the mixed ingredients
        foreach (IngredientSO ingredient in ingredientsToMix)
        {
            if (recipeIngredients.Contains(ingredient))
            {
                // Remove the ingredient from the temporary list if it matches
                recipeIngredients.Remove(ingredient);
            }
            else
            {
                // If an ingredient is not part of the recipe, it's an incorrect match
                return false;
            }
        }

        // If the temporary recipe list is empty, it's an exact match
        return recipeIngredients.Count == 0;
    }

    private void HandleFailedPotion()
    {
        // Clear the ingredients list and icons
        ingredientsToMix.Clear();
        ClearIngredientsIcon();

        potionOrderManager.GenerateOrder();

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

    potionOrderManager.GenerateOrder();
    // Reset potion ready flag
    _potionReady = false;
}

    private IEnumerator WaitToClear(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ClearIngredientsIcon();
        ScoreManager.instance.AddScore(potionRecipe.recipeRarityLevel);
        _potionReady = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Bubble"))
        {
            _audioManager.PlaySound("CauldronDrop");
            IngredientSO ingredient = other.gameObject.GetComponent<Ingredient>().ingredient;
            AddIngredient(ingredient);
            Destroy(other.gameObject);
        }
        
    }
}
