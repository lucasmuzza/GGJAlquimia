using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantScript : MonoBehaviour
{
    public merchantOffers offer;
    public ConversationSO conversationSO;
    [Header(" ")]
    [SerializeField] private AnimationClip enter;
    [SerializeField] private AnimationClip leave; 

    Animation Animation;
    float animationLenght = 2.2f;

    [SerializeField] private GameObject market;
    
 
    public void Start()
    {
        market = GameObject.FindWithTag("Market").transform.GetChild(0).gameObject;
        Animation = GetComponent<Animation>();
        
        Animation.clip = enter;
        Animation.Play();
        Invoke("StarMarket", 2f);
    }

    public void NPCLeave()
    {
        Animation.clip = leave; 
        Animation.Play();
        Destroy(gameObject, animationLenght);
    }

    
    private void StarMarket()
    {
        market.SetActive(true);
        
        for( int i = 0 ; i < market.transform.childCount; i++)
        {

            market.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = offer.offers[i].ingredient.ingredientName;
            market.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = offer.offers[i].ingredient.ingredientDescription;
            market.transform.GetChild(i).GetChild(3).GetComponent<BuyStock>().ingredientSO = offer.offers[i].ingredient;
        }
    }
}
