using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantScript : MonoBehaviour
{
    public MerchantOffers offer;
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

    
   

   
}
