using UnityEngine;

public class NPC : MonoBehaviour
{
    public ConversationSO conversationSO;
    public Potion requestedPotion;

    private void Start()
    {
        requestedPotion = conversationSO.requestedPotion;
    }
}
