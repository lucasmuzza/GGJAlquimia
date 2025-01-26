using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public float score;
    private Cauldron _cauldron;
    
    private void Awake()
    {
        if(instance == null)
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
        _cauldron = FindFirstObjectByType<Cauldron>();

        _cauldron.OnPotionMatched.AddListener(AddScore);
    }

    public void AddScore(int recipeLevel)
    {
        if(recipeLevel == 1)
        {
            score += 50;
        }
        else if(recipeLevel == 2)
        {
            score += 100;
        }
        else if(recipeLevel == 3)
        {
            score += 300;
        }
    }

    public float GetScore()
    {
        return score;
    }
}
