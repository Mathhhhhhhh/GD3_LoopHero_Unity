using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _levelCompletePanel;
    [SerializeField] private TMP_Text _congratulationsText;
    [SerializeField] private GameObject _diceButton;

    [Header("Settings")]
    [SerializeField] private string _congratulationsMessage = "Bravo !";
    [SerializeField] private string _nextSceneName = "NextLevel";

    private void Start()
    {
        if (_levelCompletePanel != null)
        {
            _levelCompletePanel.SetActive(false);
        }
    }

    public void ShowLevelComplete(string nextScene = null)
    {
        if (nextScene != null)
        {
            _nextSceneName = nextScene;
        }

        if (_levelCompletePanel != null)
        {
            _levelCompletePanel.SetActive(true);
        }

        if (_congratulationsText != null)
        {
            _congratulationsText.text = _congratulationsMessage;
        }

        if (_diceButton != null)
        {
            _diceButton.SetActive(false);
        }
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(_nextSceneName))
        {
            SceneManager.LoadScene(_nextSceneName);
        }
        else
        {
            Debug.LogWarning("Aucune scène suivante n'est définie !");
        }
    }
}
