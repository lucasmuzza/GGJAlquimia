using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Novo Dialogo", menuName = "Dialogo/Conversa")]
public class ConversaSO : ScriptableObject
{
    public FalasDaConversa[] Falas;
    public String Nome;
}

[System.Serializable]
public class FalasDaConversa
{
    
    //public Personagem Personagem;
    //public int IdDaExpressao;

    [TextArea]
    public string[] TextoDasFalas;    
}
