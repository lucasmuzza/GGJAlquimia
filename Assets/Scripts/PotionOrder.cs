using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Potion Order", fileName = "New Potion Order")]
public class PotionOrder : ScriptableObject
{
    public string potionName;
    public string potionDescription;
    public Sprite potionSprite;
    public PotionRecipeSO potionRecipe;
}
