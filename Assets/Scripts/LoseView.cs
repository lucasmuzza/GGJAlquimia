using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoseView : MonoBehaviour
{
    public UIDocument loseUIDoc;

    private Timer _timer;
    private GameManager _gameManager;
    private BubbleManager _bubbleManager;
    private IngredientStock _stockManager;
    private PotionOrderManager _potionOrderManager;

    private VisualElement _rootVisualElement;

    private Label _timeOfTheRun;

    private Button _mainMenuButton;
    private Button _quitButton;


    private void Start()
    {
        loseUIDoc = GetComponent<UIDocument>();

        _timer = Timer.instance;
        
        _rootVisualElement = loseUIDoc.rootVisualElement;

        _timeOfTheRun = _rootVisualElement.Q<Label>("TimerLabel");
        _timeOfTheRun.text = $"O tempo da run foi de: {_timer.GetElapsedTime()}";

        _mainMenuButton = _rootVisualElement.Q<Button>("MainMenuButton");
        _quitButton = _rootVisualElement.Q<Button>("QuitButton");

        _mainMenuButton.clicked += LoadMainMenu;

        _quitButton.clicked += QuitGame;
    }

    private void LoadMainMenu()
    {
        Destroy(_timer.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    private void QuitGame()
    {
        Debug.Log("Closing the game");
        Application.Quit();
    }
}
