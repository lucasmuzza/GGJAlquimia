using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private GameObject market;

    public MerchantOffers[] offer;
    MerchantOffers choosenOffer;

    [SerializeField] IngredientStock stockManager;
    [SerializeField] ScoreManager ScoreManager;

    [SerializeField] private float refreshPrice;

    void Start()
    {
        StarMarket();
        for( int i = 0 ; i < market.transform.childCount; i++)
        {
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().stockManager = stockManager;
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().scoreManager = ScoreManager;
        }
    }

    private void StarMarket()
    {  
        RandomizeMarket();
    }

    public void RefreshMarket()
    {
        if(ScoreManager.score > refreshPrice)
        {
            RandomizeMarket();
        }
    }


    private void RandomizeMarket()
    {
        for( int i = 0 ; i < market.transform.childCount; i++)
        {
            choosenOffer = offer[Random.Range(0, offer.Length)];
            market.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite =  choosenOffer.offers[i].ingredient.ingredientIcon;
            market.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = choosenOffer.offers[i].ingredient.ingredientName;
            market.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = choosenOffer.offers[i].ingredient.ingredientDescription;
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().ingredientSO = choosenOffer.offers[i].ingredient;
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().ingredientPrice = choosenOffer.offers[i].price;
            market.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text =new string("R$ " + choosenOffer.offers[i].price.ToString());
        }
    }
}
