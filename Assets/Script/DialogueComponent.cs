using UnityEngine;
using UnityEngine.Events;

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

    [Header("Timer Bonus Settings")]
    [SerializeField] protected int _movesReward = 0;
    [SerializeField] protected int _rewardAtVisit = -1;
    private bool _rewardGiven = false;

    [Header("Events")]
    [SerializeField] protected UnityEvent onDialogueComplete;

    protected bool _shouldIncrementVisit = true;

    public virtual void Action(Player CurrentPawn)
    {
        Debug.Log($"<color=cyan>[{gameObject.name}] Action() - VisitCount AVANT: {_visitCount}</color>");

        if (_visitCount >= _dialogueDatasArray.Length)
        {
            Debug.Log($"<color=yellow>[{gameObject.name}] Plus de dialogues disponibles</color>");
            return;
        }

        _currentDialogueData = GetCurrentDialogueData();

        if (_currentDialogueData != null)
        {
            _currentRowIndex = 0;
            _currentRow = _currentDialogueData.rows[_currentRowIndex];
            _shouldIncrementVisit = true;

            Debug.Log($"<color=cyan>[{gameObject.name}] Dialogue chargé : {_currentDialogueData.name}, ShouldIncrement = TRUE</color>");
            Debug.Log($"<color=yellow>[{gameObject.name}] _currentRowIndex = {_currentRowIndex}, Texte = {_currentRow.longDialogue}</color>");

            _dialogueController.StartDialogue(this);
        }
    }

    protected virtual DialogueDatas GetCurrentDialogueData()
    {
        int dialogueIndex = Mathf.Min(_visitCount, _dialogueDatasArray.Length - 1);
        Debug.Log($"<color=cyan>[{gameObject.name}] GetCurrentDialogueData - VisitCount: {_visitCount}, DialogueIndex: {dialogueIndex}, Dialogue: {_dialogueDatasArray[dialogueIndex].name}</color>");
        return _dialogueDatasArray[dialogueIndex];
    }

    public DialogueRow GetDialogueRow()
    {
        DialogueRow row = _currentDialogueData.rows[_currentRowIndex];
        Debug.Log($"<color=orange>[{gameObject.name}] GetDialogueRow() - Index: {_currentRowIndex}, RowNumber: {row.rowNumber}, Texte: {row.longDialogue}</color>");
        return row;
    }

    public string GetDialogueText()
    {
        Debug.Log($"<color=purple>[{gameObject.name}] GetDialogueText() - _currentRow.longDialogue: {_currentRow.longDialogue}</color>");
        return _currentRow.longDialogue;
    }

    public string GetCharactername()
    {
        return _currentRow.characterName;
    }

    public void SetShouldIncrementVisit(bool shouldIncrement)
    {
        Debug.Log($"<color=magenta>[{gameObject.name}] SetShouldIncrementVisit appelé avec: {shouldIncrement}</color>");
        _shouldIncrementVisit = shouldIncrement;
    }

    public virtual void GoToRow(int rowNumber)
    {
        Debug.Log($"<color=green>[{gameObject.name}] GoToRow({rowNumber}) - ShouldIncrement: {_shouldIncrementVisit}</color>");

        if (rowNumber == -1)
        {
            _dialogueController.EndDialogue();

            if (_confettiEffect != null && _visitCount == _confettiAtVisit)
            {
                PlayConfetti();
            }

            if (!_rewardGiven && _visitCount == _rewardAtVisit && _movesReward > 0 && GameManager.Instance != null && GameManager.Instance.MovementTimer != null)
            {
                GameManager.Instance.MovementTimer.AddMoves(_movesReward);
                _rewardGiven = true;
            }

            Debug.Log($"<color=orange>[{gameObject.name}] Avant incrémentation - VisitCount: {_visitCount}, ShouldIncrement: {_shouldIncrementVisit}</color>");

            if (_shouldIncrementVisit)
            {
                _visitCount++;
                Debug.Log($"<color=red>[{gameObject.name}] VisitCount INCRÉMENTÉ à: {_visitCount}</color>");
                onDialogueComplete?.Invoke();
            }
            else
            {
                Debug.Log($"<color=lime>[{gameObject.name}] VisitCount PAS incrémenté (choix refusé)</color>");
            }
        }
        else
        {
            if (rowNumber >= 0 && rowNumber < _currentDialogueData.rows.Length)
            {
                _currentRowIndex = rowNumber;
                _currentRow = _currentDialogueData.rows[_currentRowIndex];
                Debug.Log($"<color=green>[{gameObject.name}] GoToRow mis à jour - _currentRowIndex: {_currentRowIndex}, _currentRow.longDialogue: {_currentRow.longDialogue}</color>");
                _dialogueController.UpdateText();
            }
            else
            {
                Debug.LogError($"Index de ligne invalide : {rowNumber}. Le dialogue contient {_currentDialogueData.rows.Length} lignes (indices 0 à {_currentDialogueData.rows.Length - 1}).");
            }
        }
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

            if (!_rewardGiven && _visitCount == _rewardAtVisit && _movesReward > 0 && GameManager.Instance != null && GameManager.Instance.MovementTimer != null)
            {
                GameManager.Instance.MovementTimer.AddMoves(_movesReward);
                _rewardGiven = true;
            }

            Debug.Log($"<color=orange>[{gameObject.name}] GetNextRow - Avant incrémentation - VisitCount: {_visitCount}, ShouldIncrement: {_shouldIncrementVisit}</color>");

            if (_shouldIncrementVisit)
            {
                _visitCount++;
                Debug.Log($"<color=red>[{gameObject.name}] GetNextRow - VisitCount INCRÉMENTÉ à: {_visitCount}</color>");
                onDialogueComplete?.Invoke();
            }
            else
            {
                Debug.Log($"<color=lime>[{gameObject.name}] GetNextRow - VisitCount PAS incrémenté</color>");
            }
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

    public int GetVisitCount()
    {
        return _visitCount;
    }
}
