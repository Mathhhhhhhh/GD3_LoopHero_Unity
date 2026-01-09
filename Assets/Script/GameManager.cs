using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    [Header("Timer Settings")]
    [SerializeField] private MovementTimer _movementTimer;
    [SerializeField] private int _startingMoves = 20;

    [Header("Turn Settings")]
    [SerializeField] private TurnManager _turnManager;

    [Header("UI References")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _diceButton;

    public MovementTimer MovementTimer => _movementTimer;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        if (_movementTimer != null)
        {
            _movementTimer.SetMaxMoves(_startingMoves);
            _movementTimer.ResetTimer();
        }

        if (_turnManager != null)
        {
            _turnManager.ResetTurns();
        }

        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over !");

        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }

        if (_diceButton != null)
        {
            _diceButton.SetActive(false);
        }
    }

    public void RestartGame()
    {
        if (_movementTimer != null)
        {
            _movementTimer.SetMaxMoves(_startingMoves);
            _movementTimer.ResetTimer();
        }

        if (_turnManager != null)
        {
            _turnManager.ResetTurns();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
