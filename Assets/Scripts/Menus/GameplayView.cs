using UnityEngine;
using UnityEngine.UIElements;

public class GameplayView : MonoBehaviour
{
    public ScoreManager scoreManagerInstance;

    public UIDocument gameplayUIDoc;
    private VisualElement _rootVisualElement;
    private Label _moneyLabel;    
    private void Start()
    {
        scoreManagerInstance = ScoreManager.instance;

        gameplayUIDoc = GetComponent<UIDocument>();

        _rootVisualElement = gameplayUIDoc.rootVisualElement;
        
    }

    private void Update()
    {

        if(GameManager.instance.isGamePaused)
        {
            _rootVisualElement.pickingMode = PickingMode.Ignore;
        }
        else if(!GameManager.instance.isGamePaused)
        {
            _rootVisualElement.pickingMode = PickingMode.Position;
        }

        _moneyLabel = _rootVisualElement.Q<Label>("moneyLabel");
        _moneyLabel.text = $"Moedas: {scoreManagerInstance.GetScore()}";
    }
}
