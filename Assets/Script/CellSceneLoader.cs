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

    private bool _hasLoaded = false;

    public void Action(Player CurrentPawn)
    {
        if (_loadOnlyOnce && _hasLoaded)
        {
            Debug.Log("[CellSceneLoader] Déjà chargé");
            return;
        }

        if (string.IsNullOrEmpty(_sceneToLoad))
        {
            Debug.LogWarning("[CellSceneLoader] Nom de scène non défini!");
            return;
        }

        _hasLoaded = true;

        if (_uiToDisable != null)
        {
            _uiToDisable.SetActive(false);
            Debug.Log("[CellSceneLoader] UI désactivée");
        }

        Debug.Log($"[CellSceneLoader] Chargement de la scène: {_sceneToLoad}");

        if (_loadDelay > 0)
        {
            Invoke(nameof(LoadScene), _loadDelay);
        }
        else
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
