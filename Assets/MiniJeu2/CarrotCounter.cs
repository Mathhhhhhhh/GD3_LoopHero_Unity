using System;
using TMPro;
using UnityEngine;

public class CarrotCounter : MonoBehaviour
{
    public static CarrotCounter Instance { get; private set; }

    public static event Action OnAllCarrotsCollected;

    private const int TotalCarrots = 3;

    [SerializeField] private TextMeshProUGUI _counterText;

    private int _collectedCount = 0;
    public int CollectedCount => _collectedCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UpdateDisplay();
    }

    /// <summary>Incrémente le compteur, met à jour l'affichage et déclenche la victoire si toutes les carottes sont ramassées.</summary>
    public void AddCarrot()
    {
        _collectedCount++;
        Debug.Log($"[CarrotCounter] Carottes collectées : {_collectedCount}/{TotalCarrots}");
        UpdateDisplay();

        if (_collectedCount >= TotalCarrots)
        {
            OnAllCarrotsCollected?.Invoke();
        }
    }

    private void UpdateDisplay()
    {
        if (_counterText != null)
            _counterText.text = $"Carottes : {_collectedCount}/{TotalCarrots}";
    }
}
