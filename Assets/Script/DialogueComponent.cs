using UnityEngine;

public class DialogueComponent : MonoBehaviour, IActionable
{
    [SerializeField] private DialogueDatas _dialogueData;
    private DialogueRow _currentRow;
    private int _currentRowIndex;
    [SerializeField] private UiDialogueController _dialogueController;

    [Header("Déblocage")]
    [SerializeField] private GameObject _objectToActivate;

    public void Action(Player CurrentPawn)
    {
        _currentRow = GetDialogueRow();
        _dialogueController.StartDialogue(this);
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
        if (_objectToActivate != null)
        {
            _objectToActivate.SetActive(true);
            Debug.Log($"Section débloquée : {_objectToActivate.name}");

            Board board = GetComponentInParent<Board>();
            if (board != null)
            {
                board.RefreshCells();
            }
        }
    }

}
