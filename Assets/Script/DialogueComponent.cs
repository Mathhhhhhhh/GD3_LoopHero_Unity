using UnityEngine;

public class DialogueComponent : MonoBehaviour, IActionable, IDialogue
{
    [SerializeField] private DialogueDatas _dialogueData;
    private DialogueRow _currentRow;
    private int _currentRowIndex;
    [SerializeField] private UiDialogueController _dialogueController;

    [Header("Deblocage premier dialogue")]
    [SerializeField] private GameObject[] _cellsToActivateFirst;
    [SerializeField] private Board _board;
    [SerializeField] private AudioClip _unlockSound;
    [SerializeField] private Material _materialAfterFirstDialogue;
    [SerializeField] private Material _materialForFirstCells;

    [Header("Deblocage recompense")]
    [SerializeField] private GameObject[] _cellsToActivateReward;
    [SerializeField] private DialogueComponent[] _dialoguesToUnlock;
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private float _energyReward = 50f;

    [Header("Phrase d'attente")]
    [SerializeField] private string _waitingMessage = "Je n'ai rien d'autre a dire pour le moment.";
    [SerializeField] private bool _showWaitingMessage = true;

    [Header("Condition d'activation")]
    [SerializeField] private DialogueComponent _requiredQuestDialogue;

    [Header("Changement de materiau")]
    [SerializeField] private Material _materialWhenQuestCompleted;
    [SerializeField] private Material _materialForRewardCells;

    private bool _firstDialogueCompleted = false;
    private bool _questCompleted = false;
    private bool _rewardGiven = false;

    private void Start()
    {
        if (GameInstance.Instance == null) return;

        string id = gameObject.name;

        if (GameInstance.Instance.IsRewardGiven(id))
        {
            _firstDialogueCompleted = true;
            _questCompleted = true;
            _rewardGiven = true;
            RestoreFirstActivations();
            RestoreRewardActivations();
            Debug.Log($"[DialogueComponent] {id} : etat final restaure.");
        }
        else if (GameInstance.Instance.IsFirstDialogueCompleted(id))
        {
            _firstDialogueCompleted = true;
            RestoreFirstActivations();
            Debug.Log($"[DialogueComponent] {id} : premier dialogue restaure, cases reactivees.");
        }
    }

    /// <summary>Reactive silencieusement les cases du premier dialogue sans effets de bord.</summary>
    private void RestoreFirstActivations()
    {
        foreach (GameObject cell in _cellsToActivateFirst)
        {
            if (cell == null) continue;
            cell.SetActive(true);
            CellLuciole luciole = cell.GetComponent<CellLuciole>();
            if (luciole != null) luciole.EnablePower();
        }

        _board?.RefreshCells();
    }

    /// <summary>Reactive silencieusement les cases de recompense sans effets de bord.</summary>
    private void RestoreRewardActivations()
    {
        foreach (GameObject cell in _cellsToActivateReward)
        {
            if (cell == null) continue;
            cell.SetActive(true);
            CellLuciole luciole = cell.GetComponent<CellLuciole>();
            if (luciole != null) luciole.EnablePower();
        }

        foreach (DialogueComponent dialogue in _dialoguesToUnlock)
        {
            if (dialogue != null) dialogue.enabled = true;
        }

        _board?.RefreshCells();
    }

    /// <summary>Declenchee quand le joueur pose le pion sur cette case.</summary>
    public void Action(Player CurrentPawn)
    {
        if (_requiredQuestDialogue != null && !_requiredQuestDialogue.IsQuestCompleted())
            return;

        if (_rewardGiven)
            return;

        if (_questCompleted)
        {
            _currentRowIndex = 3;
            _currentRow = GetDialogueRow();
            _dialogueController.StartDialogue(this);
        }
        else if (_firstDialogueCompleted)
        {
            if (_showWaitingMessage)
                _dialogueController.ShowWaitingMessage(_waitingMessage);
        }
        else
        {
            _currentRowIndex = 0;
            _currentRow = GetDialogueRow();
            _dialogueController.StartDialogue(this);
        }
    }

    public DialogueRow GetDialogueRow() => _dialogueData.rows[_currentRowIndex];
    public string GetDialogueText() => _currentRow.longDialogue;
    public string GetCharactername() => _currentRow.characterName;

    /// <summary>Avance le dialogue a la ligne suivante ou termine la sequence.</summary>
    public void GetNextRow()
    {
        if (_currentRow.nextRowNumber == -1)
        {
            _dialogueController.EndDialogue();

            if (_questCompleted && !_rewardGiven)
            {
                ActivateRewardObjects();
                _rewardGiven = true;
                GameInstance.Instance?.RegisterRewardGiven(gameObject.name);
            }
            else if (!_questCompleted && !_firstDialogueCompleted)
            {
                ActivateFirstObjects();
                _firstDialogueCompleted = true;
                GameInstance.Instance?.RegisterFirstDialogueCompleted(gameObject.name);
            }
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
        }
    }

    /// <summary>Marque la quete comme completee depuis l'exterieur (ex: CellChamps).</summary>
    public void UnlockDialogueLine(int lineIndex)
    {
        _questCompleted = true;
    }

    public bool IsQuestCompleted() => _questCompleted;

    private void ActivateFirstObjects()
    {
        foreach (GameObject cell in _cellsToActivateFirst)
        {
            if (cell == null) continue;
            cell.SetActive(true);
        }

        if (_materialForFirstCells != null)
        {
            foreach (GameObject cell in _cellsToActivateFirst)
            {
                if (cell == null) continue;
                MeshRenderer r = cell.GetComponent<MeshRenderer>();
                if (r != null) r.sharedMaterial = _materialForFirstCells;
            }
        }

        foreach (GameObject cell in _cellsToActivateFirst)
        {
            if (cell == null) continue;
            CellLuciole luciole = cell.GetComponent<CellLuciole>();
            if (luciole != null) luciole.EnablePower();
        }

        _board?.RefreshCells();

        if (_unlockSound != null)
            AudioSource.PlayClipAtPoint(_unlockSound, transform.position);

        if (_materialAfterFirstDialogue != null)
        {
            MeshRenderer r = GetComponent<MeshRenderer>();
            if (r != null) r.sharedMaterial = _materialAfterFirstDialogue;
        }
    }

    private void ActivateRewardObjects()
    {
        foreach (GameObject cell in _cellsToActivateReward)
        {
            if (cell == null) continue;
            cell.SetActive(true);
        }

        if (_materialForRewardCells != null)
        {
            foreach (GameObject cell in _cellsToActivateReward)
            {
                if (cell == null) continue;
                MeshRenderer r = cell.GetComponent<MeshRenderer>();
                if (r != null) r.sharedMaterial = _materialForRewardCells;
            }
        }

        foreach (GameObject cell in _cellsToActivateReward)
        {
            if (cell == null) continue;
            CellLuciole luciole = cell.GetComponent<CellLuciole>();
            if (luciole != null) luciole.EnablePower();
        }

        foreach (DialogueComponent dialogue in _dialoguesToUnlock)
        {
            if (dialogue != null) dialogue.enabled = true;
        }

        if (_energyBar != null && _energyReward > 0)
            _energyBar.AddEnergy(_energyReward);

        _board?.RefreshCells();

        if (_materialWhenQuestCompleted != null)
        {
            MeshRenderer r = GetComponent<MeshRenderer>();
            if (r != null) r.sharedMaterial = _materialWhenQuestCompleted;
        }
    }
}
