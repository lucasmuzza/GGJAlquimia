using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Bubble : MonoBehaviour
{
    public BubbleManager bubbleManagerInstance;
    public IngredientSO bubbleIngrendient;
    public BoxCollider2D boxCollider;

    private void Start()
    {
        bubbleManagerInstance = BubbleManager.instance;


        boxCollider = GetComponent<BoxCollider2D>();
    }


    public void PoppingBubble()
    {
        // When the bubble pops remove the bubble from the list of bubbles to cycle
        bubbleManagerInstance.RemoveBubble(gameObject);
        Destroy(gameObject);
        GameObject poppedIngredient = Instantiate(bubbleIngrendient.ingredientPrefab,transform.position,Quaternion.identity);
    }
} 
