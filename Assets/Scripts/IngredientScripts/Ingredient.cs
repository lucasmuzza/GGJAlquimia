using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Ingredient : MonoBehaviour
{
    public IngredientSO ingredient;
    public Rigidbody2D rb;

    private AudioManager _audioManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        _audioManager = AudioManager.instance;
    }
}
