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
    private int _indiceFalas;
    private Queue<string> _filaFalas;
   

    void Awake()
    {
        Time.timeScale = 1.0f ;
        //IniciarDialogo(ConversaTeste);
        // player.transform.parent.GetChild(1).GetComponentInChildren<Playercam>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        // player.GetComponent<PlayerMovementAdvanced>().enabled = false;
    }    


    public void IniciarDialogo(ConversationSO conversa)
    {        
        //Faz aparecer a caixa de dialogo
        _DialogueBox.SetActive(true);

        //Inicializa a fila
        _filaFalas = new Queue<string>();

        _currentDialogue = conversa;
        _indiceFalas = 0;

        ProximaFala();
    }

    public void ProximaFala()
    {
        if (wordPerWord.EstaMostrando)
        {
            wordPerWord.MostrarTextoTodo();
            return;
        }

        if (_filaFalas.Count == 0)
        {
            if (_indiceFalas < _currentDialogue.words.Length)
            {
                //Coloca a imagem do personagem na caixa de diálogo e arruma o tamanho
                //_avatarPersonagem.sprite = _currentDialogue.Falas[_indiceFalas].Personagem.Expressoes[_currentDialogue.Falas[_indiceFalas].IdDaExpressao];
                //_avatarPersonagem.SetNativeSize();

                //Coloca o nome do personagem na caixa de diálogo
                _nameCharacter.text = _currentDialogue.Name;

                //Coloca todas as falas da expressão atual em uma fila
                foreach (string textoFala in _currentDialogue.words[_indiceFalas].TextLines)
                {
                    _filaFalas.Enqueue(textoFala);
                }

                _indiceFalas++;
            }
            else
            {
                //Faz sumir a caixa de diálogo
                _DialogueBox.SetActive(false);
                return;
            }
        }

        wordPerWord.MostrarTextoLetraPorLetra(_filaFalas.Dequeue());
    }

}
