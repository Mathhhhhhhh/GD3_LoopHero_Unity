using UnityEngine;

[System.Serializable]
public struct DialogueChoice
{
    public string choiceText;
    public int nextRowNumber;
    [Tooltip("Si coché, ce choix ne fera PAS progresser vers le dialogue suivant lors de la prochaine visite")]
    public bool doesNotProgress;
}

[System.Serializable]
public struct DialogueRow
{
    public int rowNumber;
    public string characterName;
    public string longDialogue;
    public int nextRowNumber;

    [Header("Multiple Choices")]
    public bool hasChoices;
    public DialogueChoice[] choices;
}

[CreateAssetMenu(fileName = "DialogueDatas", menuName = "Scriptable Objects/DialogueDatas")]
public class DialogueDatas : ScriptableObject
{
    public DialogueRow[] rows;
}
