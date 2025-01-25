using UnityEngine;

[CreateAssetMenu(fileName = "Novo Personagem", menuName = "Dialogo/Personagem")]

public class PersonagemSO : ScriptableObject
{
    public string Nome;
    public Sprite[] Expressoes;
}
