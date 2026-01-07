using UnityEngine;

public class DialogueComponent : MonoBehaviour, IActionable
{
    [SerializeField] protected DialogueDatas[] _dialogueDatasArray;
    protected DialogueDatas _currentDialogueData;
    protected DialogueRow _currentRow;
    protected int _currentRowIndex;
    private int _visitCount = 0;
    [SerializeField] protected UiDialogueController _dialogueController;

    [Header("Celebration Settings")]
    [SerializeField] protected ParticleSystem _confettiEffect;
    [SerializeField] protected int _confettiAtVisit = -1;

    public virtual void Action(Player CurrentPawn)
    {
        if (_visitCount >= _dialogueDatasArray.Length)
        {
            return;
        }

        _currentDialogueData = GetCurrentDialogueData();

        if (_currentDialogueData != null)
        {
            _currentRowIndex = 0;
            _currentRow = _currentDialogueData.rows[_currentRowIndex];
            _dialogueController.StartDialogue(this);
        }
    }

    protected virtual DialogueDatas GetCurrentDialogueData()
    {
        int dialogueIndex = Mathf.Min(_visitCount, _dialogueDatasArray.Length - 1);
        return _dialogueDatasArray[dialogueIndex];
    }

    public DialogueRow GetDialogueRow()
    {
        return _currentDialogueData.rows[_currentRowIndex];
    }

    public string GetDialogueText()
    {
        return _currentRow.longDialogue;
    }

    public string GetCharactername()
    {
        return _currentRow.characterName;
    }

    public virtual void GetNextRow()
    {
        if (_currentRow.nextRowNumber == -1)
        {
            _dialogueController.EndDialogue();

            if (_confettiEffect != null && _visitCount == _confettiAtVisit)
            {
                PlayConfetti();
            }

            _visitCount++;
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
        }
    }

    protected virtual void PlayConfetti()
    {
        _confettiEffect.Play();
    }
}
