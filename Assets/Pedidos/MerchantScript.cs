using UnityEngine;

public class MerchantScript : MonoBehaviour
{
    public merchantOffers offer;
    public ConversationSO conversationSO;
    [Header(" ")]
    [SerializeField] private AnimationClip enter;
    [SerializeField] private AnimationClip leave; 

    Animation Animation;
    float animationLenght = 2.2f;
    public void Start()
    {
        Animation = GetComponent<Animation>();
        
        Animation.clip = enter;
        Animation.Play();
    }

    public void NPCLeave()
    {
        Animation.clip = leave; 
        Animation.Play();
        Destroy(gameObject, animationLenght);
    }

}
