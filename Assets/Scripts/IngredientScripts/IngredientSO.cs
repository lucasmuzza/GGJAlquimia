using UnityEngine;


public enum IngredientRarity
{
    Common,
    Rare
}

[CreateAssetMenu(menuName = "Potions/Ingredients", fileName = "New Ingredient")]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    public string ingredientDescription;
    public IngredientRarity ingredientRarity;
    public GameObject ingredientPrefab;
    public Sprite ingredientIcon;
}
