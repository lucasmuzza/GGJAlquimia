using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fialogue", menuName = "Dialogue/Conversation")]
public class ConversationSO : ScriptableObject
{
    public ConversationLines[] words;
    public String Name;
    public Potion requestedPotion;
}

[System.Serializable]
public class ConversationLines
{
    
    public CharacterSO Personagem;
    //public int IdDaExpressao;

    [TextArea]
    public string[] TextLines;    
}
