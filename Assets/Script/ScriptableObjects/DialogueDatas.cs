using Unity.Collections;
using UnityEngine;

[System.Serializable]
public struct DialogueRow
{
    public int rowNumber; //pas vraiment utile
    public string characterName;
    public string longDialogue;
    public int nextRowNumber;
}

[CreateAssetMenu(fileName = "DialogueDatas", menuName = "Scriptable Objects/DialogueDatas")]
public class DialogueDatas : ScriptableObject
{
    public DialogueRow[] rows;
}
