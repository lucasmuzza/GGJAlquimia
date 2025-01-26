using Unity.VisualScripting;
using UnityEngine;


public class BuyStock : MonoBehaviour
{
    public IngredientSO ingredientSO;
    public IngredientStock stockManager;

    public ScoreManager scoreManager;
    public float ingredientPrice;

    public void OnButtonClick()
    {
      if(scoreManager.score < ingredientPrice)
      {
        
      }
      else
      {
          scoreManager.score = scoreManager.score-ingredientPrice;
          stockManager.AddIngredientToStock(ingredientSO);
      }

       
    }

}
