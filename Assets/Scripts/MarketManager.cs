using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private GameObject market;

    public MerchantOffers[] offer;
    MerchantOffers choosenOffer;

    [SerializeField] IngredientStock stockManager;
    [SerializeField] ScoreManager scoreManager;

    [SerializeField] private float refreshPrice;

    [SerializeField] private KeyCode openAndCloseMarket;
    private bool isMarketOpen = false;
    

    void Start()
    {
        StarMarket();
        for( int i = 0 ; i < market.transform.childCount; i++)
        {
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().stockManager = stockManager;
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().scoreManager = scoreManager;
        }
    }

    private void StarMarket()
    {  
        RandomizeMarket();
    }

    public void RefreshMarket()
    {
        if(scoreManager.score > refreshPrice)
        {
            RandomizeMarket();
        }
    }

    void Update(){
        if(Input.GetKeyDown(openAndCloseMarket) && !isMarketOpen)
        {
            market.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            isMarketOpen = true;
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(openAndCloseMarket) && isMarketOpen)
        {
            market.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            isMarketOpen = false;
            Time.timeScale = 1;
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
