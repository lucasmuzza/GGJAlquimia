using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(OptionsMenu))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Camera> cameras;
    public string[] currentView;
    public bool isGamePaused;
    public OptionsMenu optionsMenu;

    public PlayerInputHandler playerInputHandler;

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
    
    void Start()
    {
        optionsMenu = GetComponent<OptionsMenu>();      

        playerInputHandler = FindFirstObjectByType<PlayerInputHandler>();

        playerInputHandler.onGamePaused.AddListener(ToggleOptionsMenu);
    }

    void Update()
    {
        
    }

    private void ToggleOptionsMenu(bool hasPaused)
    {
        if(hasPaused)
        {
            isGamePaused = true;
            optionsMenu.rootVisualElement.style.display = DisplayStyle.Flex; // Turn on the options menu 
        }

        else if(!hasPaused)
        {
            isGamePaused = false;
            optionsMenu.rootVisualElement.style.display = DisplayStyle.None; // Turn off the options menu
        }
    }


}
