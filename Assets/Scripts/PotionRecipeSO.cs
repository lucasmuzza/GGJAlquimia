using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Recipes", fileName = "New Recipe")]
public class PotionRecipeSO : ScriptableObject
{
    public string recipeName;
    public int recipeRarityLevel;
    public Potion potionType; // This represents the final potion
    public List<IngredientSO> ingredients;
}
