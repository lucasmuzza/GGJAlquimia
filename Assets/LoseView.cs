using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoseView : MonoBehaviour
{
    public UIDocument loseUIDoc;

    private Timer _timer;

    private VisualElement _rootVisualElement;

    private Label _timeOfTheRun;

    private Button _mainMenuButton;
    private Button _quitButton;


    private void Start()
    {
        loseUIDoc = GetComponent<UIDocument>();

        _timer = Timer.instance;

        _rootVisualElement = loseUIDoc.rootVisualElement;

        _timeOfTheRun = _rootVisualElement.Q<Label>("TimeLabel");
        _timeOfTheRun.text = $"O tempo da run foi de: {_timer.GetElapsedTime()}";

        _mainMenuButton = _rootVisualElement.Q<Button>("MainMenuButton");
        _quitButton = _rootVisualElement.Q<Button>("QuitButton");

        _mainMenuButton.clicked += LoadMainMenu;

        _quitButton.clicked += QuitGame;
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
