using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject _DialogueBox;

    // [SerializeField] private Image _avatarPersonagem;
    [SerializeField] private SistemaDialogoCadenciado wordPerWord;
    [SerializeField] private TextMeshProUGUI _nameCharacter;
    [SerializeField] private TextMeshProUGUI _textWord;

    private ConversationSO _currentDialogue;
    public ConversationSO DialogueTest;
    private int _indexSpeech;
    private Queue<string> _wordsLine;
   

    void Awake()
    {
        Time.timeScale = 1.0f ;
        //IniciarDialogo(ConversaTeste);
        // player.transform.parent.GetChild(1).GetComponentInChildren<Playercam>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        // player.GetComponent<PlayerMovementAdvanced>().enabled = false;
    }    


    public void IniciarDialogo(ConversationSO conversation)
    {        
        //Faz aparecer a caixa de dialogo
        _DialogueBox.SetActive(true);

        //Inicializa a fila
        _wordsLine = new Queue<string>();

        _currentDialogue = conversation;
        _indexSpeech = 0;

        NextWord();
    }

    public void NextWord()
    {
        if (wordPerWord.IsShowing)
        {
            wordPerWord.ShowAllText();
            return;
        }

        if (_wordsLine.Count == 0)
        {
            if (_indexSpeech < _currentDialogue.words.Length)
            {
                //Coloca a imagem do personagem na caixa de diálogo e arruma o tamanho
                //_avatarPersonagem.sprite = _currentDialogue.Falas[_indiceFalas].Personagem.Expressoes[_currentDialogue.Falas[_indiceFalas].IdDaExpressao];
                //_avatarPersonagem.SetNativeSize();

                //Coloca o nome do personagem na caixa de diálogo
                _nameCharacter.text = _currentDialogue.Name;

                //Coloca todas as falas da expressão atual em uma fila
                foreach (string textWord in _currentDialogue.words[_indexSpeech].TextLines)
                {
                    _wordsLine.Enqueue(textWord);
                }

                _indexSpeech++;
            }
            else
            {
                //Faz sumir a caixa de diálogo
                _DialogueBox.SetActive(false);
                return;
            }
        }

        wordPerWord.ShowTextLetterPerLetter(_wordsLine.Dequeue());
    }

}
