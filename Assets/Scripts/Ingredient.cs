using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Ingredient : MonoBehaviour
{
    public IngredientSO ingredient;
    public Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Cauldron"))
        {
            other.gameObject.GetComponent<Cauldron>().AddIngredient(ingredient);
            Destroy(gameObject);
        }
        
    }
}
