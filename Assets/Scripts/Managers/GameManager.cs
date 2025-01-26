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

    public int orderSucessAmount;
    public int orderFailedAmount;

    public PlayerInputHandler playerInputHandler;


    private Timer timer;

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

        cameras[1].gameObject.SetActive(false);
    }
    
    void Start()
    {
        optionsMenu = GetComponent<OptionsMenu>();      

        playerInputHandler = FindFirstObjectByType<PlayerInputHandler>();

        playerInputHandler.onGamePaused.AddListener(ToggleOptionsMenu);
        playerInputHandler.onChangedSceneView.AddListener(ChangeCameraView);

        timer = FindFirstObjectByType<Timer>();
    }

    private void Update()
    {
        if(orderSucessAmount >= 3)
        {
            // Incentive for getting at least 3 in a row
            orderFailedAmount = 0;
        }

        if(orderFailedAmount >= 5)
        {
            // Lose the game
            timer.StopTimer();
        }
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

    private void ChangeCameraView(string currentView)
    {
        if(!isGamePaused)
        {
            foreach (Camera camera in cameras)
            {
                camera.gameObject.SetActive(false); // Disable each camera
            }

            // Now, enable the camera based on the current view string
            foreach (Camera camera in cameras)
            {
                if (camera.name == currentView)
                {
                    camera.gameObject.SetActive(true);
                    break; 
                }
            }
        }
    }
}
