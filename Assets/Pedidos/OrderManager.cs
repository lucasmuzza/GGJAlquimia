using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OrderManager : MonoBehaviour
{
    public ConversationSO[] orders;

    public GameObject[] npcs;
    public LayerMask npcLayer;


    public Transform spawnLocation;
    private GameObject instantiatedNPCs;

    private AudioManager _audioManager;
    private DialogueSystem _dialogueSystem;

    private bool _npcActive = false;
    
    void Start()
    {
        _dialogueSystem = FindFirstObjectByType<DialogueSystem>();

        _audioManager = AudioManager.instance;

        if(!_npcActive)
        {
            instantiatedNPCs = Instantiate(npcs[Random.Range(0, npcs.Length)],spawnLocation.position, Quaternion.identity);

            // Passes a random order from the orders array
            instantiatedNPCs.GetComponent<NPC>().conversationSO = orders[Random.Range(0, orders.Length)]; 
            _npcActive = true;
        }
    }

    public void OnPotionSucess()
    {
        _audioManager.PlaySound("PotionSucess");
        Destroy(instantiatedNPCs);
        _npcActive = false;
    }

    public void OnPotionFailed()
    {
        _audioManager.PlaySound("PotionFailure");
        Destroy(instantiatedNPCs);
        _npcActive = false;

    }
}
