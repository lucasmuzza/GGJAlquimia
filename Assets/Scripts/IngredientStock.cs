using System.Collections.Generic;
using UnityEngine;

public class IngredientStock : MonoBehaviour
{
    [Header("Stock Lists")]
    public List<IngredientSO> initialIngredientsStock;
    public List<IngredientSO> currentStock;

    [Space(10)]

    [Header("Variables")]
    [SerializeField] private int initialIngredientsAmount;
    [SerializeField] private float commonProbability = 0.7f;
    [SerializeField] private float rareProbability = 0.3f;

    [Space(10)]

    [Header("Ingredients Pools")]
    [SerializeField] private List<IngredientSO> commonIngredientPool;
    [SerializeField] private List<IngredientSO> rareIngredientPool;


    private void Start()
    {
        SelectInitialStock(initialIngredientsAmount);

        currentStock = new List<IngredientSO>(initialIngredientsStock);
    }

    /// <summary>
    /// Add an ingrendientSO to the current stock
    /// </summary>
    /// <param name="ingredientToAdd"></param>
    public void AddIngredientToStock(IngredientSO ingredientToAdd)
    {
        currentStock.Add(ingredientToAdd);
    }

    /// <summary>
    /// Remove an ingredientSO of the current stock
    /// </summary>
    /// <param name="ingredientToRemove"></param>
    public void RemoveIngredientFromStock(IngredientSO ingredientToRemove)
    {
        currentStock.Remove(ingredientToRemove);
    }

    /// <summary>
    /// Get the list of ingredients of the current stock
    /// </summary>
    /// <returns></returns>
    public List<IngredientSO> GetCurrentStock()
    {
        return currentStock;
    }

    public void SelectInitialStock(int totalIngredients)
    {
        initialIngredientsStock = new List<IngredientSO>();

        for (int i = 0; i < totalIngredients; i++)
        {
            float randomValue = Random.Range(0f, 1f);

            if (randomValue >= commonProbability && commonIngredientPool.Count > 0)
            {
                // Pick a random ingredient from the common pool
                int randomIndex = Random.Range(0, commonIngredientPool.Count);
                IngredientSO selectedIngredient = commonIngredientPool[randomIndex];
                initialIngredientsStock.Add(selectedIngredient);
            }
            else if (randomValue <= rareProbability && rareIngredientPool.Count > 0)
            {
                // Pick a random ingredient from the rare pool
                int randomIndex = Random.Range(0, rareIngredientPool.Count);
                IngredientSO selectedIngredient = rareIngredientPool[randomIndex];
                initialIngredientsStock.Add(selectedIngredient);
            }
        }
    }
}
