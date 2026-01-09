using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [Header("Timer References")]
    [SerializeField] private MovementTimer _movementTimer;
    [SerializeField] private TurnManager _turnManager;

    [Header("Hours Display")]
    [SerializeField] private TMP_Text _hoursText;
    [SerializeField] private string _hoursFormat = "Temps restant : {0} heures";

    [Header("Turns Display")]
    [SerializeField] private TMP_Text _turnsText;
    [SerializeField] private string _turnsFormat = "Tours : {0} / {1}";

    private void Start()
    {
        UpdateTimerDisplay();
    }

    private void Update()
    {
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (_movementTimer != null && _hoursText != null)
        {
            _hoursText.text = string.Format(_hoursFormat, _movementTimer.RemainingMoves);
        }

        if (_turnManager != null && _turnsText != null)
        {
            _turnsText.text = string.Format(_turnsFormat,
                _turnManager.CurrentTurn,
                _turnManager.MaxTurns);
        }
    }
}
