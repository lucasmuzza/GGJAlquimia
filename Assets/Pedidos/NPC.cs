using UnityEngine;

public class NPC : MonoBehaviour
{
    public ConversationSO conversationSO;

    [SerializeField]
    private AnimationClip enter;
    [SerializeField]
    private AnimationClip leave; 
    Animation Animation;
    float animationLenght = 2.2f;

    GameObject market;


    public void Start()
    {
        Animation = GetComponent<Animation>();

        
        Animation.clip = enter;
        Animation.Play();
        Invoke("OpenMarket", 2f);
    }

    public void NPCLeave()
    {
        Animation.clip = leave; 
        Animation.Play();
        Destroy(gameObject, animationLenght);
    }

    void OpenMarket()
    {
        market.SetActive(true);
        
    }
}
