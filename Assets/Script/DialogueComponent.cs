using UnityEngine;

public class DialogueComponent : MonoBehaviour, IActionable
{
    [SerializeField] private DialogueDatas _dialogueData;
    private DialogueRow _currentRow;
    private int _currentRowIndex;
    [SerializeField] private UiDialogueController _dialogueController;

    [Header("Déblocage")]
    [SerializeField] private GameObject[] _cellsToActivate;
    [SerializeField] private Board _board;

    [Header("Phrase d'attente")]
    [SerializeField] private string _waitingMessage = "Je n'ai rien d'autre à dire pour le moment.";

    private bool _dialogueCompleted = false;

    public void Action(Player CurrentPawn)
    {
        if (_dialogueCompleted)
        {
            _dialogueController.ShowWaitingMessage(_waitingMessage);
        }
        else
        {
            _currentRow = GetDialogueRow();
            _dialogueController.StartDialogue(this);
        }
    }

    public DialogueRow GetDialogueRow()
    {
        return _dialogueData.rows[_currentRowIndex];
    }

    public string GetDialogueText()
    {
        return _currentRow.longDialogue;
    }

    public string GetCharactername()
    {
        return _currentRow.characterName;
    }

    public void GetNextRow()
    {
        if (_currentRow.nextRowNumber == -1)
        {
            _dialogueController.EndDialogue();
            ActivateObject();
            _dialogueCompleted = true;
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
        }
    }

    private void ActivateObject()
    {
        foreach (GameObject cell in _cellsToActivate)
        {
            if (cell != null)
            {
                cell.SetActive(true);
            }
        }

        if (_board != null)
        {
            _board.RefreshCells();
        }
    }
}
