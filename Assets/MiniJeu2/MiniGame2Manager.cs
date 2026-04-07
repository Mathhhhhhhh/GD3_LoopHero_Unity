using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Gère la condition de fin du mini-jeu : affiche le message de victoire et charge la scène suivante.</summary>
public class MiniGame2Manager : MonoBehaviour
{
    private const string NextSceneName = "Dev_map";
    private const float DelayBeforeLoad = 5f;

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

    private void Start()
    {
        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);
    }

    /// <summary>Déclenché quand toutes les carottes sont collectées.</summary>
    private void HandleVictory()
    {
        Debug.Log("[MiniGame2Manager] 🎉 Mini-jeu terminé ! Chargement de Dev_map dans 5 secondes.");

        if (_victoryPanel != null)
            _victoryPanel.SetActive(true);

        StartCoroutine(LoadNextSceneRoutine());
    }

    private IEnumerator LoadNextSceneRoutine()
    {
        yield return new WaitForSeconds(DelayBeforeLoad);
        SceneManager.LoadScene(NextSceneName);
    }
}
