using UnityEngine;
using UnityEngine.SceneManagement;

public class CellSceneLoader : MonoBehaviour, IActionable
{
    [Header("Configuration")]
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private bool _loadOnlyOnce = true;
    [SerializeField] private float _loadDelay = 0.5f;

    [Header("UI")]
    [SerializeField] private GameObject _uiToDisable;

    [Header("Action au retour")]
    [SerializeField] private GameObject[] _cellsToDeactivateOnReturn;
    [SerializeField] private string _returnSpawnCellName;
    [SerializeField] private string[] _cellNamesToActivateOnReturn;
    [SerializeField] private Board _board;

    [Header("Déclenchement différé (sans scène à charger)")]
    [Tooltip("Si défini, l'action de retour se déclenche quand cette case a déjà chargé sa scène.")]
    [SerializeField] private string _triggeredByCellId;

    private bool _hasLoaded = false;
    private bool _returnActionDone = false;

    private void Start()
    {
        if (GameInstance.Instance == null) return;

        string id = gameObject.name;

        if (_loadOnlyOnce && GameInstance.Instance.IsSceneAlreadyLoaded(id))
            _hasLoaded = true;

        if (GameInstance.Instance.IsReturnActionDone(id))
        {
            _returnActionDone = true;
            DeactivateReturnCells();
        }
    }

    /// <summary>Charge la scène associée, ou exécute l'action de retour si la scène a déjà été chargée.</summary>
    public void Action(Player currentPawn)
    {
        // Mode déclenchement différé : cette case n'a pas de scène mais réagit quand une autre a été visitée
        if (!string.IsNullOrEmpty(_triggeredByCellId))
        {
            if (GameInstance.Instance != null && GameInstance.Instance.IsSceneAlreadyLoaded(_triggeredByCellId))
            {
                if (!_returnActionDone)
                {
                    _returnActionDone = true;
                    GameInstance.Instance?.RegisterReturnAction(gameObject.name);
                    DeactivateReturnCells();
                }
            }
            return;
        }

        // Mode normal
        if (_hasLoaded)
        {
            if (!_returnActionDone)
            {
                _returnActionDone = true;
                GameInstance.Instance?.RegisterReturnAction(gameObject.name);
                DeactivateReturnCells();
            }
            return;
        }

        if (string.IsNullOrEmpty(_sceneToLoad))
        {
            Debug.LogWarning("[CellSceneLoader] Nom de scène non défini !");
            return;
        }

        _hasLoaded = true;

        if (_loadOnlyOnce)
            GameInstance.Instance?.RegisterSceneLoaded(gameObject.name);

        if (!string.IsNullOrEmpty(_returnSpawnCellName))
            GameInstance.Instance?.SetSpawnCell(_returnSpawnCellName);

        if (_cellNamesToActivateOnReturn != null && _cellNamesToActivateOnReturn.Length > 0)
            GameInstance.Instance?.SetCellsToActivateOnReturn(_cellNamesToActivateOnReturn);

        if (_uiToDisable != null)
            _uiToDisable.SetActive(false);

        if (_loadDelay > 0)
            Invoke(nameof(LoadScene), _loadDelay);
        else
            LoadScene();
    }

    private void DeactivateReturnCells()
    {
        foreach (GameObject cell in _cellsToDeactivateOnReturn)
        {
            if (cell != null) cell.SetActive(false);
        }

        _board?.RefreshCells();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
