using TMPro;
using UnityEngine;

/// <summary>Affiche le nombre de carottes rapportées depuis le mini-jeu dans la Dev_map.</summary>
public class CarrotHUD : MonoBehaviour
{
    [SerializeField] private GameObject _carrotPanel;
    [SerializeField] private TextMeshProUGUI _carrotText;

    private void Start()
    {
        if (GameInstance.Instance == null || !GameInstance.Instance.HasCarrotsFromMiniGame())
        {
            if (_carrotPanel != null) _carrotPanel.SetActive(false);
            return;
        }

        int count = GameInstance.Instance.GetCarrotsFromMiniGame();
        if (_carrotText != null)
            _carrotText.text = $"Carottes : {count}";

        if (_carrotPanel != null) _carrotPanel.SetActive(true);
    }

    /// <summary>Cache le panneau et vide le compteur (appelé après remise des carottes au vieux).</summary>
    public void ClearCarrots()
    {
        GameInstance.Instance?.ClearCarrotsFromMiniGame();
        if (_carrotPanel != null) _carrotPanel.SetActive(false);
    }
}
