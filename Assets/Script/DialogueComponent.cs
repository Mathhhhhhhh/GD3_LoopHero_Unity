using UnityEngine;
using UnityEngine.Events;

public class DialogueComponent : MonoBehaviour, IActionable
{
    [SerializeField] protected DialogueDatas[] _dialogueDatasArray;
    protected DialogueDatas _currentDialogueData;
    protected DialogueRow _currentRow;
    protected int _currentRowIndex;

    private int _currentDialogueIndex = 0;
    private bool _currentDialogueShouldProgress = true;
    private bool _currentDialogueRefused = false;

    [SerializeField] protected UiDialogueController _dialogueController;

    [Header("Celebration Settings")]
    [SerializeField] protected ParticleSystem _confettiEffect;
    [SerializeField] protected int _confettiAtVisit = -1;

    [Header("Timer Bonus Settings")]
    [SerializeField] protected int _movesReward = 0;
    [SerializeField] protected int _rewardAtVisit = -1;
    private bool _rewardGiven = false;

    [Header("Events")]
    [SerializeField] protected UnityEvent onDialogueComplete;

    public virtual void Action(Player CurrentPawn)
    {
        Debug.Log($"<color=cyan>[{gameObject.name}] Action() - DialogueIndex: {_currentDialogueIndex}, Refused: {_currentDialogueRefused}</color>");

        if (_currentDialogueIndex >= _dialogueDatasArray.Length)
        {
            Debug.Log($"<color=yellow>[{gameObject.name}] Plus de dialogues disponibles</color>");
            return;
        }

        if (_currentDialogueRefused)
        {
            Debug.Log($"<color=yellow>[{gameObject.name}] Dialogue déjà refusé - aucune interaction</color>");
            return;
        }

        _currentDialogueData = _dialogueDatasArray[_currentDialogueIndex];

        if (_currentDialogueData != null)
        {
            _currentRowIndex = 0;
            _currentRow = _currentDialogueData.rows[_currentRowIndex];
            _currentDialogueShouldProgress = true;

            Debug.Log($"<color=cyan>[{gameObject.name}] Dialogue chargé : {_currentDialogueData.name}, Index: {_currentDialogueIndex}</color>");
            Debug.Log($"<color=yellow>[{gameObject.name}] Row {_currentRowIndex}: {_currentRow.longDialogue}</color>");

            _dialogueController.StartDialogue(this);
        }
    }

    public DialogueRow GetDialogueRow()
    {
        DialogueRow row = _currentDialogueData.rows[_currentRowIndex];
        Debug.Log($"<color=orange>[{gameObject.name}] GetDialogueRow() - Index: {_currentRowIndex}, RowNumber: {row.rowNumber}, Texte: {row.longDialogue}</color>");
        return row;
    }

    public string GetDialogueText()
    {
        Debug.Log($"<color=purple>[{gameObject.name}] GetDialogueText() - {_currentRow.longDialogue}</color>");
        return _currentRow.longDialogue;
    }

    public string GetCharactername()
    {
        return _currentRow.characterName;
    }

    public virtual void GoToRow(int rowNumber, bool shouldProgress)
    {
        Debug.Log($"<color=green>[{gameObject.name}] GoToRow({rowNumber}) - ShouldProgress: {shouldProgress}</color>");

        _currentDialogueShouldProgress = shouldProgress;

        if (rowNumber == -1)
        {
            EndCurrentDialogue();
        }
        else
        {
            if (rowNumber >= 0 && rowNumber < _currentDialogueData.rows.Length)
            {
                _currentRowIndex = rowNumber;
                _currentRow = _currentDialogueData.rows[_currentRowIndex];
                Debug.Log($"<color=green>[{gameObject.name}] GoToRow mis à jour - Index: {_currentRowIndex}, Texte: {_currentRow.longDialogue}</color>");
                _dialogueController.UpdateText();
            }
            else
            {
                Debug.LogError($"Index de ligne invalide : {rowNumber}. Le dialogue contient {_currentDialogueData.rows.Length} lignes.");
            }
        }
    }

    public virtual void GetNextRow()
    {
        if (_currentRow.nextRowNumber == -1)
        {
            EndCurrentDialogue();
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
        }
    }

    protected virtual void EndCurrentDialogue()
    {
        Debug.Log($"<color=orange>[{gameObject.name}] EndCurrentDialogue - ShouldProgress: {_currentDialogueShouldProgress}</color>");

        _dialogueController.EndDialogue();

        if (_confettiEffect != null && _currentDialogueIndex == _confettiAtVisit)
        {
            PlayConfetti();
        }

        if (!_rewardGiven && _currentDialogueIndex == _rewardAtVisit && _movesReward > 0 && GameManager.Instance != null && GameManager.Instance.MovementTimer != null)
        {
            GameManager.Instance.MovementTimer.AddMoves(_movesReward);
            _rewardGiven = true;
        }

        if (_currentDialogueShouldProgress)
        {
            _currentDialogueIndex++;
            _currentDialogueRefused = false;
            Debug.Log($"<color=red>[{gameObject.name}] PROGRESSION vers dialogue {_currentDialogueIndex}</color>");
            onDialogueComplete?.Invoke();
        }
        else
        {
            _currentDialogueRefused = true;
            Debug.Log($"<color=lime>[{gameObject.name}] Dialogue refusé - DialogueIndex reste à {_currentDialogueIndex}, Refused: {_currentDialogueRefused}</color>");
        }
    }

    protected virtual void PlayConfetti()
    {
        _confettiEffect.Play();
    }

    public int GetCurrentDialogueIndex()
    {
        return _currentDialogueIndex;
    }
}
