using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OrderManager : MonoBehaviour
{
    public ConversationSO[] orders;
    [SerializeField] private GameObject[] npcs;
    [SerializeField] private KeyCode nextNpc;
    [SerializeField] private KeyCode givePotion;


    [SerializeField] Transform instantiateLocation;
    [SerializeField]private GameObject instantiatedNPCs;

    
    [SerializeField] LayerMask npcLayer;
    DialogueSystem dialogueSystem;

    bool npcActive = false;
    
    void Start()
    {
        dialogueSystem = GameObject.FindWithTag("dialogueSystem").GetComponent<DialogueSystem>();
       
    }

   
  

    void Update()
    {
        if(Input.GetKeyDown(nextNpc) && !npcActive)
        {
            instantiatedNPCs = Instantiate(npcs[Random.Range(0, npcs.Length)]);
            //Cria um pedido aleatoria dentro do Array
            instantiatedNPCs.GetComponent<NPC>().conversationSO = orders[Random.Range(0, orders.Length)];
            npcActive = true;
        }
        if(Input.GetKeyDown(givePotion) && npcActive)
        {
            PotionGive();
            npcActive = false;
        }
        if(Input.GetMouseButton(0))
        {
            ShootRay();
        }
    }
    public void PotionGive()
    {
        Destroy(instantiatedNPCs);
    }

    void ShootRay()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Lança o raio na posição do mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, npcLayer);

        if (hit.collider != null)
        {
            Debug.Log("acertouNpc");
            if (hit.collider.CompareTag("npc"))
            {
                Debug.Log("acertouNpc");
                dialogueSystem.IniciarDialogo(hit.collider.gameObject.GetComponent<NPC>().conversationSO);
            }
        }
    }
}
