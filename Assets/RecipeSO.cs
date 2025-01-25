using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Recipes", fileName = "New Recipe")]
public class RecipeSO : MonoBehaviour
{
    public string recipeName;
    public int recipeRarityLevel;
    public List<IngredientSO> ingredients;
}
