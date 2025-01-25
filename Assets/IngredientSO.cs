using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Ingredients", fileName = "New Ingredient")]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    public string ingredientDescription;
    public GameObject ingredientPrefab;
    public Material ingredientMaterial;
}
