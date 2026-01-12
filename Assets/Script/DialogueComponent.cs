using UnityEngine;

public class DialogueComponent : MonoBehaviour, IActionable
{
    [SerializeField] private DialogueDatas _dialogueData;
    private DialogueRow _currentRow;
    private int _currentRowIndex;
    [SerializeField] private UiDialogueController _dialogueController;

    [Header("Déblocage premier dialogue")]
    [SerializeField] private GameObject[] _cellsToActivateFirst;
    [SerializeField] private Board _board;
    [SerializeField] private AudioClip _unlockSound;
    [SerializeField] private Material _materialAfterFirstDialogue;
    [SerializeField] private Material _materialForFirstCells;

    [Header("Déblocage récompense")]
    [SerializeField] private GameObject[] _cellsToActivateReward;
    [SerializeField] private DialogueComponent[] _dialoguesToUnlock;
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private float _energyReward = 50f;

    [Header("Phrase d'attente")]
    [SerializeField] private string _waitingMessage = "Je n'ai rien d'autre à dire pour le moment.";
    [SerializeField] private bool _showWaitingMessage = true;

    [Header("Condition d'activation")]
    [SerializeField] private DialogueComponent _requiredQuestDialogue;

    [Header("Changement de matériau")]
    [SerializeField] private Material _materialWhenQuestCompleted;
    [SerializeField] private Material _materialForRewardCells;

    private bool _firstDialogueCompleted = false;
    private bool _questCompleted = false;
    private bool _rewardGiven = false;

    public void Action(Player CurrentPawn)
    {
        if (_requiredQuestDialogue != null && !_requiredQuestDialogue.IsQuestCompleted())
        {
            return;
        }

        Debug.Log($"[DialogueComponent] Action - firstCompleted:{_firstDialogueCompleted}, questCompleted:{_questCompleted}, rewardGiven:{_rewardGiven}");

        if (_rewardGiven)
        {
            Debug.Log("[DialogueComponent] Quête terminée - case désactivée");
            return;
        }

        if (_questCompleted)
        {
            Debug.Log("[DialogueComponent] Démarrage dialogue récompense (row 3)");
            _currentRowIndex = 3;
            _currentRow = GetDialogueRow();
            _dialogueController.StartDialogue(this);
        }
        else if (_firstDialogueCompleted)
        {
            if (_showWaitingMessage)
            {
                Debug.Log("[DialogueComponent] Affichage message d'attente");
                _dialogueController.ShowWaitingMessage(_waitingMessage);
            }
            else
            {
                Debug.Log("[DialogueComponent] Premier dialogue terminé - case inactive");
                return;
            }
        }
        else
        {
            Debug.Log("[DialogueComponent] Démarrage dialogue initial (row 0)");
            _currentRowIndex = 0;
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
        Debug.Log($"[DialogueComponent] GetNextRow - currentRow nextNumber: {_currentRow.nextRowNumber}");

        if (_currentRow.nextRowNumber == -1)
        {
            Debug.Log($"[DialogueComponent] Fin dialogue - questCompleted:{_questCompleted}, rewardGiven:{_rewardGiven}");
            _dialogueController.EndDialogue();

            if (_questCompleted && !_rewardGiven)
            {
                Debug.Log("[DialogueComponent] *** ACTIVATION DES OBJETS DE RÉCOMPENSE ***");
                ActivateRewardObjects();
                _rewardGiven = true;
            }
            else if (!_questCompleted && !_firstDialogueCompleted)
            {
                Debug.Log("[DialogueComponent] *** ACTIVATION DES CHAMPS (toutes les cases) ***");
                ActivateFirstObjects();
                _firstDialogueCompleted = true;
            }
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
        }
    }

    public void UnlockDialogueLine(int lineIndex)
    {
        Debug.Log($"[DialogueComponent] UnlockDialogueLine appelée - index:{lineIndex}");
        _questCompleted = true;
    }

    public bool IsQuestCompleted()
    {
        return _questCompleted;
    }

    private void ActivateFirstObjects()
    {
        Debug.Log($"[DialogueComponent] ActivateFirstObjects - cellules à activer: {_cellsToActivateFirst.Length}");

        foreach (GameObject cell in _cellsToActivateFirst)
        {
            if (cell != null)
            {
                Debug.Log($"[DialogueComponent] Activation de {cell.name}");
                cell.SetActive(true);
            }
        }

        if (_materialForFirstCells != null)
        {
            Debug.Log($"[DialogueComponent] Application du matériau: {_materialForFirstCells.name}");

            foreach (GameObject cell in _cellsToActivateFirst)
            {
                if (cell != null)
                {
                    MeshRenderer cellRenderer = cell.GetComponent<MeshRenderer>();
                    if (cellRenderer != null)
                    {
                        Debug.Log($"[DialogueComponent] AVANT changement - {cell.name} a le matériau: {cellRenderer.sharedMaterial.name}");
                        cellRenderer.sharedMaterial = _materialForFirstCells;
                        Debug.Log($"[DialogueComponent] APRES changement - {cell.name} a le matériau: {cellRenderer.sharedMaterial.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"[DialogueComponent] {cell.name} n'a PAS de MeshRenderer!");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("[DialogueComponent] _materialForFirstCells est NULL!");
        }

        foreach (GameObject cell in _cellsToActivateFirst)
        {
            if (cell != null)
            {
                CellLuciole luciole = cell.GetComponent<CellLuciole>();
                if (luciole != null)
                {
                    luciole.EnablePower();
                }
            }
        }

        if (_board != null)
        {
            Debug.Log("[DialogueComponent] RefreshCells du Board");
            _board.RefreshCells();
        }

        if (_unlockSound != null)
        {
            AudioSource.PlayClipAtPoint(_unlockSound, transform.position);
        }

        if (_materialAfterFirstDialogue != null)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = _materialAfterFirstDialogue;
                Debug.Log("[DialogueComponent] Changement de matériau après premier dialogue");
            }
        }
    }

    private void ActivateRewardObjects()
    {
        Debug.Log($"[DialogueComponent] ActivateRewardObjects - cellules à activer: {_cellsToActivateReward.Length}");

        foreach (GameObject cell in _cellsToActivateReward)
        {
            if (cell != null)
            {
                Debug.Log($"[DialogueComponent] Activation de {cell.name}");
                cell.SetActive(true);
            }
        }

        if (_materialForRewardCells != null)
        {
            foreach (GameObject cell in _cellsToActivateReward)
            {
                if (cell != null)
                {
                    MeshRenderer cellRenderer = cell.GetComponent<MeshRenderer>();
                    if (cellRenderer != null)
                    {
                        cellRenderer.sharedMaterial = _materialForRewardCells;
                        Debug.Log($"[DialogueComponent] Changement de matériau pour {cell.name}");
                    }
                }
            }
        }

        foreach (GameObject cell in _cellsToActivateReward)
        {
            if (cell != null)
            {
                CellLuciole luciole = cell.GetComponent<CellLuciole>();
                if (luciole != null)
                {
                    luciole.EnablePower();
                }
            }
        }

        foreach (DialogueComponent dialogue in _dialoguesToUnlock)
        {
            if (dialogue != null)
            {
                Debug.Log($"[DialogueComponent] Déblocage dialogue de {dialogue.gameObject.name}");
                dialogue.enabled = true;
            }
        }

        if (_energyBar != null && _energyReward > 0)
        {
            _energyBar.AddEnergy(_energyReward);
            Debug.Log($"[DialogueComponent] Récompense d'énergie donnée: +{_energyReward}");
        }

        if (_board != null)
        {
            Debug.Log("[DialogueComponent] RefreshCells du Board");
            _board.RefreshCells();
        }

        if (_materialWhenQuestCompleted != null)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = _materialWhenQuestCompleted;
                Debug.Log("[DialogueComponent] Changement de matériau effectué");
            }
        }
    }
}
