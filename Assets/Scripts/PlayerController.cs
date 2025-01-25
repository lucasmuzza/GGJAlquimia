using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 mousePosition;
    public Vector3 worldPosition;



    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();

        worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)); 

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            PopBubble();
        }    
    }

    private void PopBubble()
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition,Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Bubble>(out Bubble bubble))
        {
            bubble.PoppingBubble();
        }
        
        else
        {
            Debug.Log($"The object that was hit is: {hit.collider.gameObject}");
        }
        
    }
}
