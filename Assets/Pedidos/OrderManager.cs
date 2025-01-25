using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OrderManager : MonoBehaviour
{
    public ConversationSO[] orders;
    [SerializeField] private GameObject[] npcs;

    [Header(" ")]
    [SerializeField] private GameObject merchant;
    [SerializeField] private merchantOffers[] merchantOffers;
    [SerializeField] private int turnsToAppear;
    [SerializeField] private int merchantTotalTurns;

    
    [Header(" ")]
    [SerializeField] private KeyCode nextNpc;
    [SerializeField] private KeyCode givePotion;

    [Header(" ")]
    [SerializeField] LayerMask npcLayer;
    [SerializeField] Transform instantiateLocation;
    
    private GameObject instantiatedNPC;
    private GameObject instantiatedMerchant;

    
    
    DialogueSystem dialogueSystem;

    bool canSpawnNPC = true;
    bool canDeleteNPC = false;
    
    void Start()
    {
        merchantTotalTurns = turnsToAppear;
        dialogueSystem = GameObject.FindWithTag("dialogueSystem").GetComponent<DialogueSystem>();
    }

   
  

    void Update()
    {
        if(Input.GetKeyDown(nextNpc) && canSpawnNPC)
        {
            canDeleteNPC = false;
            CreateNPC();
        }
        if(Input.GetKeyDown(givePotion) && !canSpawnNPC && canDeleteNPC)
        {
            PotionGive();
            canSpawnNPC = true;
        }
        if(Input.GetMouseButton(0))
        {
            ShootRay();
        }
    }
    public void PotionGive()
    {
        if(instantiatedNPC != null)
        instantiatedNPC.GetComponent<NPC>().NPCLeave();
        else if(instantiatedMerchant!= null)
        instantiatedMerchant.GetComponent<MerchantScript>().NPCLeave();
    }
    void CreateNPC()
    {
        canSpawnNPC = false;
        if(turnsToAppear > 0)
        {
            instantiatedNPC = Instantiate(npcs[Random.Range(0, npcs.Length)]);
            //Cria um pedido aleatoria dentro do Array
            instantiatedNPC.GetComponent<NPC>().conversationSO = orders[Random.Range(0, orders.Length)];
            Invoke("NPCActive", 2f);
            turnsToAppear--;
        }
        else
        {
            instantiatedMerchant = Instantiate(merchant);
            instantiatedMerchant.GetComponent<MerchantScript>().offer = merchantOffers[Random.Range(0 , merchantOffers.Length)]; 
            turnsToAppear = merchantTotalTurns;
            Invoke("NPCActive", 2f);
        }
    }

    void NPCActive()
    {
        canDeleteNPC = true;
    }
    void ShootRay()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Lança o raio na posição do mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, npcLayer);

        if (hit.collider != null)
        {
            
            if (hit.collider.CompareTag("npc"))
            {
                dialogueSystem.IniciarDialogo(hit.collider.gameObject.GetComponent<NPC>().conversationSO);
            }
            // if(hit.collider.CompareTag("merchant"))
            // {
            //     dialogueSystem.IniciarDialogo(hit.collider.gameObject.GetComponent<MerchantScript>().conversationSO);
            // }
        }
    }
}
