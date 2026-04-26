using TMPro;
using UnityEngine;

/// <summary>
/// Gère l'affichage UI du mini-jeu cache-cache :
/// - Nom de la phase courante
/// - Timer en secondes
/// - Message de fin
/// </summary>
public class HideAndSeekUI : MonoBehaviour
{
    [Header("Phase")]
    [SerializeField] private TextMeshProUGUI _phaseText;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Fin de partie")]
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TextMeshProUGUI _endText;

    private void Start()
    {
        if (_endPanel != null)
            _endPanel.SetActive(false);
    }

    /// <summary>Affiche le nom de la phase et initialise le timer.</summary>
    public void ShowPhase(string phaseName, float duration)
    {
        if (_phaseText != null)
            _phaseText.text = phaseName;

        if (_endPanel != null)
            _endPanel.SetActive(false);

        UpdateTimer(duration);
    }

    /// <summary>Met à jour l'affichage du timer chaque frame.</summary>
    public void UpdateTimer(float timeLeft)
    {
        if (_timerText == null) return;

        timeLeft = Mathf.Max(0f, timeLeft);
        int seconds = Mathf.CeilToInt(timeLeft);
        _timerText.text = $"{seconds}s";
    }

    /// <summary>Affiche le panneau de fin avec le message résultat.</summary>
    public void ShowEndMessage(string message)
    {
        if (_phaseText != null)
            _phaseText.text = string.Empty;

        if (_timerText != null)
            _timerText.text = string.Empty;

        if (_endPanel != null)
            _endPanel.SetActive(true);

        if (_endText != null)
            _endText.text = message;
    }
}
