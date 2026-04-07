using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameEnd : MonoBehaviour
{
    private const string DevMapSceneName = "Dev_map";
    private const float ReturnDelay = 5f;

    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private TextMeshProUGUI _victoryText;

    private void OnEnable()
    {
        CarrotCounter.OnAllCarrotsCollected += HandleVictory;
    }

    private void OnDisable()
    {
        CarrotCounter.OnAllCarrotsCollected -= HandleVictory;
    }

    private void HandleVictory()
    {
        Debug.Log("[MiniGameEnd] 🏆 Mini-jeu terminé ! Retour vers Dev_map dans 5 secondes.");

        // Sauvegarder le nombre de carottes collectées dans GameInstance
        if (GameInstance.Instance != null && CarrotCounter.Instance != null)
            GameInstance.Instance.SetCarrotsFromMiniGame(CarrotCounter.Instance.CollectedCount);

        if (_victoryPanel != null)
            _victoryPanel.SetActive(true);

        if (_victoryText != null)
            _victoryText.text = "Bien joué !\nRetour dans 5 secondes...";

        StartCoroutine(ReturnToDevMapRoutine());
    }

    /// <summary>Attend 5 secondes puis charge la scène Dev_map.</summary>
    private IEnumerator ReturnToDevMapRoutine()
    {
        yield return new WaitForSeconds(ReturnDelay);
        SceneManager.LoadScene(DevMapSceneName);
    }
}
