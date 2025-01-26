using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Vector2 mousePosition;
    public Vector3 worldPosition;

    public LayerMask bubbleLayer;
    public LayerMask clientLayer;

    public PlayerInputHandler playerInputHandler;
    public DialogueSystem dialogueSystem;

    private void Start()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        dialogueSystem = FindFirstObjectByType<DialogueSystem>(); 
    }

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();

        worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)); 

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            MouseButtonClicked();
        }
    }

    private void MouseButtonClicked()
    {
        if(Camera.main.name == "PotionView")
        {
            PopBubble();
        }
        else if(Camera.main.name == "ClientView")
        {
            TalkToClient();
        }
    }

    private void PopBubble()
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition,Vector2.zero, Mathf.Infinity, bubbleLayer);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Bubble>(out Bubble bubble))
        {
            bubble.PoppingBubble();
        }
    }

    private void TalkToClient()
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, clientLayer);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<NPC>(out NPC npc))
        {
            dialogueSystem.IniciarDialogo(npc.conversationSO);
        }
    }
}
