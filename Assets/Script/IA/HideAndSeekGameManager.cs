using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Orchestre les phases du mini-jeu cache-cache.
/// Phase Hiding (20s) : joueur se cache, IA inactive.
/// Phase Seeking (30s) : IA cherche le joueur, joueur immobile.
/// Phase End : résultat affiché.
/// </summary>
public class HideAndSeekGameManager : MonoBehaviour
{
    public static HideAndSeekGameManager Instance { get; private set; }

    public enum GamePhase { Hiding, Seeking, Found, NotFound }

    [Header("Durées")]
    [SerializeField] private float _hidingDuration = 20f;
    [SerializeField] private float _seekingDuration = 30f;
    [SerializeField] private float _restartDelay = 3f;

    [Header("Scènes")]
    [SerializeField] private string _endSceneName = "IA_End";

    [Header("Références")]
    [SerializeField] private HideAndSeekIAController _iaController;
    [SerializeField] private HideAndSeekPlayerController _playerController;
    [SerializeField] private HideAndSeekUI _ui;

    private GamePhase _currentPhase = GamePhase.Hiding;
    private float _timer;

    public GamePhase CurrentPhase => _currentPhase;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnterPhase(GamePhase.Hiding);
    }

    private void Update()
    {
        if (_currentPhase == GamePhase.Found || _currentPhase == GamePhase.NotFound)
            return;

        _timer -= Time.deltaTime;
        _ui.UpdateTimer(_timer);

        if (_timer <= 0f)
        {
            if (_currentPhase == GamePhase.Hiding)
                EnterPhase(GamePhase.Seeking);
            else if (_currentPhase == GamePhase.Seeking)
                EnterPhase(GamePhase.NotFound);
        }
    }

    private void EnterPhase(GamePhase phase)
    {
        _currentPhase = phase;

        switch (phase)
        {
            case GamePhase.Hiding:
                _timer = _hidingDuration;
                _iaController.SetActive(false);
                _playerController.SetMovementEnabled(true);
                _ui.ShowPhase("Cachez-vous !", _hidingDuration);
                break;

            case GamePhase.Seeking:
                _timer = _seekingDuration;
                _iaController.SetActive(true);
                _playerController.SetMovementEnabled(false);
                _ui.ShowPhase("Ne bougez plus !", _seekingDuration);
                break;

            case GamePhase.Found:
                _iaController.SetActive(false);
                _playerController.SetMovementEnabled(false);
                _ui.ShowEndMessage("Vous avez été trouvé !");
                StartCoroutine(RestartAfterDelay());
                break;

            case GamePhase.NotFound:
                _iaController.SetActive(false);
                _playerController.SetMovementEnabled(false);
                _ui.ShowEndMessage("Vous n'avez pas été trouvé !");
                StartCoroutine(LoadEndSceneAfterDelay());
                break;
        }
    }

    /// <summary>Appelé par HideAndSeekIAController quand l'IA détecte le joueur pendant Seeking.</summary>
    public void OnPlayerFound()
    {
        if (_currentPhase != GamePhase.Seeking) return;
        EnterPhase(GamePhase.Found);
    }

    /// <summary>Attend _restartDelay secondes puis recharge la scène courante.</summary>
    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(_restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>Attend _restartDelay secondes puis charge la scène de fin (victoire).</summary>
    private IEnumerator LoadEndSceneAfterDelay()
    {
        yield return new WaitForSeconds(_restartDelay);
        SceneManager.LoadScene(_endSceneName);
    }
}
