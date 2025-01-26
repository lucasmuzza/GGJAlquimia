using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Bubble : MonoBehaviour
{
    public BubbleManager bubbleManagerInstance;
    public IngredientSO bubbleIngrendient;
    public BoxCollider2D boxCollider;

    private AudioManager _audioManager;

    private void Start()
    {
        bubbleManagerInstance = BubbleManager.instance;

        _audioManager = AudioManager.instance;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void PoppingBubble()
    {
        // When the bubble pops remove the bubble from the list of bubbles to cycle
        bubbleManagerInstance.RemoveBubble(gameObject);  

        _audioManager.PlaySound("Bubble");

        GameObject poppedIngredient = Instantiate(bubbleIngrendient.ingredientPrefab,transform.position,Quaternion.identity);
        poppedIngredient.GetComponent<Ingredient>().ingredient = bubbleIngrendient;
        Destroy(gameObject);
    }
} 
