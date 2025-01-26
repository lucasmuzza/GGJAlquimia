using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public static Timer instance; // Singleton instance


    private GameplayView _gameplayView;
    private UIDocument _gameplayUIDoc;
    private VisualElement _rootVisualElemnt;
    private Label _timerLabel;
    private float elapsedTime; 
    private bool isRunning;
    private bool hasPaused;

    
    public UnityEvent OnTimerStopped = new UnityEvent(); // Event triggered when the timer stops

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _gameplayView = FindFirstObjectByType<GameplayView>();
        _gameplayUIDoc = _gameplayView.GetComponent<UIDocument>();

        _rootVisualElemnt = _gameplayUIDoc.rootVisualElement;
        _timerLabel = _rootVisualElemnt.Q<Label>("TimerLabel");

        StartTimer();
    }

    private void Update()
    {
        if(GameManager.instance.isGamePaused && !hasPaused)
        {
            PauseTimer();
        }

        else if(!GameManager.instance.isGamePaused && hasPaused)
        {
            ResumeTimer();
        }

        if (isRunning)
        {
            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            _timerLabel.text = elapsedTime.ToString("F2");
        }
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        elapsedTime = 0f; // Reset elapsed time
        isRunning = true; // Start the timer
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
            OnTimerStopped?.Invoke();
        }
    }

    /// <summary>
    /// Pauses the timer without resetting the elapsed time.
    /// </summary>
    public void PauseTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resumes the timer after being paused.
    /// </summary>
    public void ResumeTimer()
    {
        isRunning = true;
    }

    /// <summary>
    /// Gets the elapsed time.
    /// </summary>
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
