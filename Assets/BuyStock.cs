using UnityEngine;


public class BuyStock : MonoBehaviour
{
    public IngredientSO ingredientSO;
    [SerializeField] IngredientStock stockManager;

    public void OnButtonClick()
    {
      stockManager.AddIngredientToStock(ingredientSO); 
    }

}
