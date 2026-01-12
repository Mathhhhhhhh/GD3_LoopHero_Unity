using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _maxEnergy = 100f;
    [SerializeField] private float _currentEnergy = 100f;

    [Header("UI")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _energyText;

    [Header("Couleurs")]
    [SerializeField] private Color _fullColor = Color.green;
    [SerializeField] private Color _mediumColor = Color.yellow;
    [SerializeField] private Color _lowColor = Color.red;

    private void Start()
    {
        UpdateEnergyBar();
    }

    public void AddEnergy(float amount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy + amount, 0, _maxEnergy);
        UpdateEnergyBar();
        Debug.Log($"[EnergyBar] Énergie ajoutée: +{amount}. Total: {_currentEnergy}/{_maxEnergy}");
    }

    public void RemoveEnergy(float amount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy - amount, 0, _maxEnergy);
        UpdateEnergyBar();
        Debug.Log($"[EnergyBar] Énergie retirée: -{amount}. Total: {_currentEnergy}/{_maxEnergy}");
    }

    public void SetEnergy(float amount)
    {
        _currentEnergy = Mathf.Clamp(amount, 0, _maxEnergy);
        UpdateEnergyBar();
    }

    public float GetCurrentEnergy()
    {
        return _currentEnergy;
    }

    public bool HasEnergy(float amount)
    {
        return _currentEnergy >= amount;
    }

    private void UpdateEnergyBar()
    {
        if (_fillImage != null)
        {
            float fillAmount = _currentEnergy / _maxEnergy;
            _fillImage.fillAmount = fillAmount;

            if (fillAmount > 0.6f)
                _fillImage.color = _fullColor;
            else if (fillAmount > 0.3f)
                _fillImage.color = _mediumColor;
            else
                _fillImage.color = _lowColor;
        }

        if (_energyText != null)
        {
            _energyText.text = $"{_currentEnergy:F0} / {_maxEnergy:F0}";
        }
    }
}
