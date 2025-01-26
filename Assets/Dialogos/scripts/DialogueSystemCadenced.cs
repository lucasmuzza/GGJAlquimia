using System.Collections;
using UnityEngine;
using TMPro;

public class SistemaDialogoCadenciado : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _textVelocity = 0.02f;

    public bool IsShowing { get; private set; }

    private IEnumerator _coroutineDoEfeito;

    public void ShowTextLetterPerLetter(string DialogueText)
    {
        _text.text = DialogueText;

        _coroutineDoEfeito = LetterPerLetterEffect();
        StartCoroutine(_coroutineDoEfeito);
        IsShowing = true;
    }

    public void ShowAllText()
    {
        StopCoroutine(_coroutineDoEfeito);
        _text.maxVisibleCharacters = _text.text.Length;

        IsShowing = false;
    }

    private IEnumerator LetterPerLetterEffect()
    {
        int caracteresTotais = _text.text.Length;
        _text.maxVisibleCharacters = 0;

        for (int i = 0; i <= caracteresTotais; i++)
        {
            _text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(_textVelocity);
        }
        IsShowing = false;
    }
}
