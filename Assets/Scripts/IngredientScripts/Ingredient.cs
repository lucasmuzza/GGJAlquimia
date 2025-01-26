using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Ingredient : MonoBehaviour
{
    public IngredientSO ingredient;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private AudioManager _audioManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = ingredient.ingredientIcon;

        _audioManager = AudioManager.instance;
    }
}
