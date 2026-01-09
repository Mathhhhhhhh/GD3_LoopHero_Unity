using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private MovementTimer _movementTimer;

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
        if (_movementTimer != null && _timerText != null)
        {
            _timerText.text = $"Temps restant : {_movementTimer.RemainingMoves} heures";
        }
    }
}
