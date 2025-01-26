using UnityEngine;

[CreateAssetMenu(fileName = "New Offer", menuName = "merchant/offer")]
public class MerchantOffers : ScriptableObject
{
    public Offers[] offers;
}
[System.Serializable]
public class Offers
{
    public IngredientSO ingredient;
    public float price;  
}
    

