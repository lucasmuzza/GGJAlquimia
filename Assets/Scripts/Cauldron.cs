using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Cauldron : MonoBehaviour
{

    [SerializeField] private List<IngredientSO> _ingredientsToMix = new List<IngredientSO>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddIngredient(IngredientSO ingredient)
    {
        _ingredientsToMix.Add(ingredient);
    }
}
