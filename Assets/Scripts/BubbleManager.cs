using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager instance;
    public IngredientStock ingredientStockInstance;

    public Transform bubbleObjectPooling;
    public Transform startWaypoint;
    public Transform endWaypoint;

    public GameObject bubblePrefab;

    public float bubblesMovementSpeed;
    public float spawnInterval = 2f; // Time interval between bubble activations

    public int maxBubblesAmount;
    public int currentBubblesAmount;

    [Header("Lists")]
    [SerializeField] private List<IngredientSO> _ingredientsForBubbles;
    [SerializeField] private List<GameObject> _generatedBubbles;
    public Queue<GameObject> bubbleQueue = new Queue<GameObject>(); // Queue for bubble management

    private bool isSpawningPaused = false; // Flag to pause/resume the spawning coroutine

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ingredientStockInstance = IngredientStock.instance;
        _ingredientsForBubbles = ingredientStockInstance.GetCurrentStock();

        _generatedBubbles = new List<GameObject>();

        if (_ingredientsForBubbles != null)
        {
            maxBubblesAmount = _ingredientsForBubbles.Count;
            currentBubblesAmount = maxBubblesAmount;
        }

        GenerateBubbles();

        // Start activating bubbles at intervals
        StartCoroutine(ActivateBubblesWithInterval());

        // Subscribe to the game pause event
        GameManager.instance.playerInputHandler.onGamePaused.AddListener(OnGamePauseChanged);
    }

    private void GenerateBubbles()
    {
        List<IngredientSO> tempBubbles = new List<IngredientSO>(_ingredientsForBubbles);

        for (int i = 0; i < maxBubblesAmount; i++)
        {
            AddBubble(tempBubbles[i]);
        }
    }

    private void Update()
    {
        if (!GameManager.instance.isGamePaused)
        {
            MoveActiveBubbles();
        }
    }

    private void MoveActiveBubbles()
    {
        foreach (var bubble in _generatedBubbles)
        {
            if (bubble.activeSelf)
            {
                bubble.transform.position = Vector3.MoveTowards(
                    bubble.transform.position,
                    endWaypoint.position,
                    bubblesMovementSpeed * Time.deltaTime
                );

                // If the bubble reaches the end waypoint, reset it
                if (Vector3.Distance(bubble.transform.position, endWaypoint.position) < 0.1f)
                {
                    ResetBubble(bubble);
                }
            }
        }
    }

    private void ResetBubble(GameObject bubble)
    {
        bubble.SetActive(false); // Deactivate the bubble
        bubble.transform.position = startWaypoint.position; // Reset position

        // Re-enqueue the bubble
        bubbleQueue.Enqueue(bubble);
    }

    private IEnumerator ActivateBubblesWithInterval()
    {
        while (bubbleQueue.Count > 0)
        {
            // Pause the coroutine if spawning is paused
            while (isSpawningPaused)
            {
                yield return null; // Wait until the game is unpaused
            }

            // Get the next bubble from the queue
            GameObject bubble = bubbleQueue.Dequeue();
            bubble.SetActive(true); // Activate the bubble

            // Wait for the spawn interval before activating the next bubble
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnGamePauseChanged(bool isPaused)
    {
        if (isPaused)
        {
            isSpawningPaused = true;
        }
        else
        {
            isSpawningPaused = false;
        }
    }

    public void AddBubble(IngredientSO ingredient)
    {
        // Instantiate the bubble prefab at the start waypoint
        GameObject bubble = Instantiate(bubblePrefab, startWaypoint.position, Quaternion.identity, bubbleObjectPooling);
        Bubble bubbleComponent = bubble.GetComponent<Bubble>();

        // Assign the ingredient to the bubble
        bubbleComponent.bubbleIngrendient = ingredient;

        // Pass the ingredientSO to the ingredient inside the bubble
        bubbleComponent.bubbleIngrendient.ingredientPrefab.GetComponent<Ingredient>().ingredient = ingredient;

        bubble.SetActive(false); // Deactivate the bubble initially

        // Add the new bubble to the generated bubbles list and the queue
        _generatedBubbles.Add(bubble);
        bubbleQueue.Enqueue(bubble);

        currentBubblesAmount++;
    }

    public void RemoveBubble(GameObject bubble)
    {
        if (bubbleQueue.Contains(bubble))
        {
            Queue<GameObject> tempQueue = new Queue<GameObject>();

            // Rebuild the queue without the removed bubble
            while (bubbleQueue.Count > 0)
            {
                GameObject dequeuedBubble = bubbleQueue.Dequeue();
                if (dequeuedBubble != bubble)
                {
                    tempQueue.Enqueue(dequeuedBubble);
                }
            }

            bubbleQueue = tempQueue;
        }

        // Removes from the generated bubbles and remove the ingredient from that bubble from the current stock and reduces the current bubbles amount
        _generatedBubbles.Remove(bubble);
        ingredientStockInstance.RemoveIngredientFromStock(bubble.GetComponent<Bubble>().bubbleIngrendient);
        currentBubblesAmount--;
    }
}
