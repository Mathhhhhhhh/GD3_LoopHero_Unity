using TMPro;
using UnityEngine;

/// <summary>
/// Dialogue de remise des carottes au vieux. Se déclenche uniquement si le joueur
/// revient du mini-jeu avec des carottes. Appelle CarrotHUD.ClearCarrots() une fois terminé.
/// </summary>
public class CarrotQuestDialogue : MonoBehaviour, IActionable, IDialogue
{
    [SerializeField] private UiDialogueController _dialogueController;
    [SerializeField] private DialogueDatas _dialogueData;
    [SerializeField] private CarrotHUD _carrotHUD;

    [Header("Matériaux")]
    [Tooltip("Matériau vert appliqué sur le Vieux pour signaler la quête disponible au retour.")]
    [SerializeField] private Material _materialQuestReady;
    [Tooltip("Matériau jaune normal remis sur le Vieux ET l'Agriculteur après remise des carottes.")]
    [SerializeField] private Material _materialReset;
    [SerializeField] private GameObject _agriculteurCell;

    [Header("Case à débloquer après la quête")]
    [SerializeField] private GameObject[] _cellsToActivateAfterQuest;
    [SerializeField] private Board _board;

    private int _currentRowIndex = 0;
    private DialogueRow _currentRow;
    private bool _rewardGiven = false;

    private void Start()
    {
        if (GameInstance.Instance == null) return;

        if (GameInstance.Instance.IsRewardGiven(gameObject.name))
        {
            _rewardGiven = true;
            return;
        }

        // Au retour du mini-jeu, colorier le Vieux en vert pour signaler la quête
        if (GameInstance.Instance.HasCarrotsFromMiniGame() && _materialQuestReady != null)
        {
            MeshRenderer r = GetComponent<MeshRenderer>();
            if (r != null) r.sharedMaterial = _materialQuestReady;
        }
    }

    /// <summary>Déclenche le dialogue uniquement si le joueur a des carottes à remettre.</summary>
    public void Action(Player currentPawn)
    {
        if (_rewardGiven) return;
        if (GameInstance.Instance == null || !GameInstance.Instance.HasCarrotsFromMiniGame()) return;

        _currentRowIndex = 0;
        _currentRow = _dialogueData.rows[_currentRowIndex];
        _dialogueController.StartDialogue(this);
    }

    public string GetDialogueText() => _currentRow.longDialogue;
    public string GetCharactername() => _currentRow.characterName;

    /// <summary>Avance le dialogue ou le termine et applique les effets de fin.</summary>
    public void GetNextRow()
    {
        if (_currentRow.nextRowNumber == -1)
        {
            _dialogueController.EndDialogue();
            _rewardGiven = true;
            GameInstance.Instance?.RegisterRewardGiven(gameObject.name);

            // Remettre le Vieux en jaune normal
            if (_materialReset != null)
            {
                MeshRenderer r = GetComponent<MeshRenderer>();
                if (r != null) r.sharedMaterial = _materialReset;
            }

            // Remettre l'Agriculteur en jaune normal
            if (_agriculteurCell != null && _materialReset != null)
            {
                MeshRenderer r = _agriculteurCell.GetComponent<MeshRenderer>();
                if (r != null) r.sharedMaterial = _materialReset;
            }

            _carrotHUD?.ClearCarrots();

            foreach (GameObject cell in _cellsToActivateAfterQuest)
            {
                if (cell != null) cell.SetActive(true);
            }

            _board?.RefreshCells();
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = _dialogueData.rows[_currentRowIndex];
            _dialogueController.UpdateText();
        }
    }
}
