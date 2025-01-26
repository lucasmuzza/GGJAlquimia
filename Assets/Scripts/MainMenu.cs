using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public UIDocument mainMenuUIDoc;
    public OptionsMenu optionsMenu;

    private AudioManager _audioManager;

    private VisualElement _rootVisualElement;

    private Button _playButton;
    private Button _optionsButton;
    private Button _quitButton;
    void Start()
    {
        mainMenuUIDoc = GetComponent<UIDocument>();

        optionsMenu = FindFirstObjectByType<OptionsMenu>();

        _audioManager = AudioManager.instance;

        _rootVisualElement = mainMenuUIDoc.rootVisualElement;

        _playButton = _rootVisualElement.Q<Button>("PlayButton");
        _optionsButton = _rootVisualElement.Q<Button>("OptionsButton");
        _quitButton = _rootVisualElement.Q<Button>("QuitButton");

        _playButton.clicked += Play;
        _optionsButton.clicked += OpenOption;
        _quitButton.clicked += Quit;
    }

    private void Play()
    {
        SceneManager.LoadScene("Gameplay");

        _audioManager.PlaySound("Test");
    }

    private void OpenOption()
    {
        _audioManager.PlaySound("Test");
        optionsMenu.rootVisualElement.style.display = DisplayStyle.Flex;


    }

    private void Quit()
    {
        Application.Quit();

        _audioManager.PlaySound("Test");

    }
}
