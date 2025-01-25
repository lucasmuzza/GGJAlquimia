using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]

public class CharacterSO : ScriptableObject
{
    public string Name;
    public Sprite[] Expressions;
}
