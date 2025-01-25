using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject _caixaDeDialogo;

    // [SerializeField] private Image _avatarPersonagem;
    [SerializeField] private SistemaDialogoCadenciado falaPorFala;
    [SerializeField] private TextMeshProUGUI _nomePersonagem;
    [SerializeField] private TextMeshProUGUI _textoFala;

    private ConversationSO _conversaAtual;
    public ConversationSO ConversaTeste;
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
        _caixaDeDialogo.SetActive(true);

        //Inicializa a fila
        _filaFalas = new Queue<string>();

        _conversaAtual = conversa;
        _indiceFalas = 0;

        ProximaFala();
    }

    public void ProximaFala()
    {
        if (falaPorFala.EstaMostrando)
        {
            falaPorFala.MostrarTextoTodo();
            return;
        }

        if (_filaFalas.Count == 0)
        {
            if (_indiceFalas < _conversaAtual.words.Length)
            {
                //Coloca a imagem do personagem na caixa de diálogo e arruma o tamanho
                //_avatarPersonagem.sprite = _conversaAtual.Falas[_indiceFalas].Personagem.Expressoes[_conversaAtual.Falas[_indiceFalas].IdDaExpressao];
                //_avatarPersonagem.SetNativeSize();

                //Coloca o nome do personagem na caixa de diálogo
                _nomePersonagem.text = _conversaAtual.Name;

                //Coloca todas as falas da expressão atual em uma fila
                foreach (string textoFala in _conversaAtual.words[_indiceFalas].TextLines)
                {
                    _filaFalas.Enqueue(textoFala);
                }

                _indiceFalas++;
            }
            else
            {
                //Faz sumir a caixa de diálogo
                _caixaDeDialogo.SetActive(false);
                return;
            }
        }

        falaPorFala.MostrarTextoLetraPorLetra(_filaFalas.Dequeue());
    }

}
