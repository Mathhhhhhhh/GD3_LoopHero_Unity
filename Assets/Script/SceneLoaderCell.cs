using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderCell : MonoBehaviour, IActionable
{
    [Header("Scene Settings")]
    [SerializeField] private string _sceneToLoad = "NextLevel";
    [SerializeField] private int _requiredVisits = 3;
    [SerializeField] private float _loadDelay = 2f;

    [Header("UI Settings")]
    [SerializeField] private GameObject _diceButton;

    [Header("Celebration Settings")]
    [SerializeField] private ParticleSystem _confettiEffect;

    private int _visitCount = 0;
    private bool _isLoading = false;

    public void Action(Player CurrentPawn)
    {
        if (_isLoading)
        {
            return;
        }

        _visitCount++;
        Debug.Log($"Passage n°{_visitCount} sur la cellule de départ.");

        if (_visitCount >= _requiredVisits)
        {
            Debug.Log($"{_requiredVisits} passages effectués ! Chargement de la scène '{_sceneToLoad}' dans {_loadDelay} secondes...");
            _isLoading = true;

            if (_confettiEffect != null)
            {
                _confettiEffect.Play();
            }

            StartCoroutine(LoadSceneWithDelay());
        }
    }

    private System.Collections.IEnumerator LoadSceneWithDelay()
    {
        if (_diceButton != null)
        {
            _diceButton.SetActive(false);
        }

        yield return new WaitForSeconds(_loadDelay);

        SceneManager.LoadScene(_sceneToLoad);
    }
}
