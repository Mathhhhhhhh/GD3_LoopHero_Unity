using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Contrôle l'écran de fin du cache-cache.
/// Affiche le message de victoire avec un fondu entrant, puis propose de revenir au menu.
/// </summary>
public class EndScreenController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Paramètres")]
    [SerializeField] private string _message = "Merci pour tout !";
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private string _menuSceneName = "Dev_map";

    private void Start()
    {
        if (_messageText != null)
            _messageText.text = _message;

        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>Fondu entrant de l'écran de fin.</summary>
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Clamp01(elapsed / _fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = 1f;
    }

    /// <summary>Retourne à la scène menu principale.</summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(_menuSceneName);
    }
}
