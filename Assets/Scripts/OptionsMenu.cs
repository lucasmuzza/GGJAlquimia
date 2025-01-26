using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OptionsMenu : MonoBehaviour
{
    public UIDocument optionsMenuUIDoc;
    public VisualTreeAsset bindTemplate;

    public VisualElement rootVisualElement;
    private VisualElement _bindsContainer;

    private TabView _tabView;

    private Button _mainMenuButton;
    private Button _saveBindsButton;
    private Button _resetBindsButton;
    private Button _changeBindButton;

    private AudioManager _audioManager;
    
    private RebindingDisplay _rebindingDisplay;
    private List<InputActionReference> _defaultActions = new List<InputActionReference>();
    public Dictionary<int,Label> _bindTextDic = new Dictionary<int, Label>();

    void Start()
    {
        optionsMenuUIDoc = GetComponent<UIDocument>();

        rootVisualElement = optionsMenuUIDoc.rootVisualElement;
        rootVisualElement.style.display = DisplayStyle.None;

        _tabView = rootVisualElement.Q<TabView>("tabs");
        
        _bindsContainer = _tabView.Q<VisualElement>("bindsContainer");

        _rebindingDisplay = GetComponent<RebindingDisplay>();
        _rebindingDisplay.OnBindUpdate.AddListener(UpdatingRebind);

        _defaultActions = _rebindingDisplay.GetDefaultBinds();

        _mainMenuButton = rootVisualElement.Q<Button>("mainMenuButton");
        _mainMenuButton.clicked += LoadMainMenu;

        _saveBindsButton = _bindsContainer.Q<Button>("saveButton");
        _saveBindsButton.clicked += SaveBinds;

        _resetBindsButton = _bindsContainer.Q<Button>("resetButton");
        _resetBindsButton.clicked += ResetBinds;

        _audioManager = AudioManager.instance;

        DisplayDefaultBinds();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                rootVisualElement.style.display = DisplayStyle.None;
            }
        }
    }


    //TODO: Change the sound to the button sound
    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

        _audioManager.PlaySound("Button");
    }

    #region  Rebinding Tab Functions
    private void DisplayDefaultBinds()
    {
        for (int i = 0; i < _defaultActions.Count; i++)
        {
            var bindTemplateVisualElement = bindTemplate.Instantiate();

            Label _bindTitle = bindTemplateVisualElement.Q<Label>("bindTitle");
            _bindTitle.text = $"{_defaultActions[i].action.name}";
            _bindTitle.AddToClassList("bindTitle");

            Label _currentBind = bindTemplateVisualElement.Q<Label>("currentBind");
            string effectivePath = _defaultActions[i].action.bindings[0].effectivePath;

            _currentBind.text = $"{InputControlPath.ToHumanReadableString(effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice)}";
            _currentBind.AddToClassList("currentBind");

            _bindTextDic.Add(i,_currentBind);

            _changeBindButton = bindTemplateVisualElement.Q<Button>("configBindButton");
            _changeBindButton.text = "Change Bind";
            
            // Pass the specific InputActionReference to the Rebind method
            var actionReference = GetActionReferenceByPath(effectivePath);
            _changeBindButton.clicked += () => Rebind(actionReference, _currentBind);

            _bindsContainer.Add(bindTemplateVisualElement);
        }

        _bindsContainer.MarkDirtyRepaint();
    }

    private InputActionReference GetActionReferenceByPath(string effectivePath)
    {
        foreach (var actionReference in _defaultActions)
        {
            foreach (var binding in actionReference.action.bindings)
            {
                if (binding.effectivePath == effectivePath)
                {
                    return actionReference;
                }
            }
        }

        Debug.LogWarning($"No InputActionReference found for effectivePath: {effectivePath}");
        return null;
    }

    private void Rebind(InputActionReference actionReference, Label currentBind)
    {

       _audioManager.PlaySound("Button");

        currentBind.text = "Waiting for Input....";

        if (actionReference != null)
        {
            _rebindingDisplay.StartRebinding(actionReference, currentBind);
        }
        else
        {
            Debug.LogWarning("Attempted to rebind with a null InputActionReference.");
        }

        _bindsContainer.MarkDirtyRepaint();
    }

    private void UpdatingRebind(string oldBindText, string newBindText)
    {
        foreach (var entry in _bindTextDic)
        {
            // Find the label associated with the previously bound key
            if (entry.Value.text == oldBindText)
            {
                entry.Value.text = newBindText;
                break;
            }
        }

        _bindsContainer.MarkDirtyRepaint();
        Debug.Log($"Rebinding updated from: {oldBindText} to: {newBindText}");
    }
    
    private void SaveBinds()
    {
        _rebindingDisplay.Save();

       _audioManager.PlaySound("Button");
    }

    private void ResetBinds()
    {
        _rebindingDisplay.ResetBinds();

        _audioManager.PlaySound("Button");

    }
    #endregion
}
