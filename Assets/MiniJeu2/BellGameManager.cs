using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellGameManager : MonoBehaviour
{
    public static BellGameManager Instance { get; private set; }

    private static readonly int[] CorrectSequence = { 2, 1, 3 };

    [Header("Récompense")]
    [SerializeField] private GameObject[] _carrots;
    [SerializeField] private float _rewardRevealDelay = 0.5f;

    [Header("Sons")]
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _successSound;

    private readonly List<int> _playerSequence = new List<int>();
    private bool _puzzleSolved = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        HideCarrots();
    }

    /// <summary>Enregistre l'activation d'une cloche et vérifie immédiatement la séquence.</summary>
    public void RegisterBellActivation(int bellId)
    {
        if (_puzzleSolved) return;

        _playerSequence.Add(bellId);
        Debug.Log($"[BellGameManager] Cloche {bellId} activée — Séquence actuelle : [{string.Join(", ", _playerSequence)}]");

        if (!IsCurrentSequenceValid())
        {
            Debug.LogWarning("[BellGameManager] ❌ Mauvaise séquence ! Réinitialisation.");
            PlaySound(_errorSound);
            ResetSequence();
            return;
        }

        if (_playerSequence.Count == CorrectSequence.Length)
        {
            OnPuzzleSolved();
        }
    }

    /// <summary>Vérifie que la séquence en cours correspond au début de la séquence correcte.</summary>
    private bool IsCurrentSequenceValid()
    {
        for (int i = 0; i < _playerSequence.Count; i++)
        {
            if (_playerSequence[i] != CorrectSequence[i])
                return false;
        }

        return true;
    }

    /// <summary>Appelé quand le joueur complète la bonne séquence.</summary>
    private void OnPuzzleSolved()
    {
        _puzzleSolved = true;
        Debug.Log("[BellGameManager] ✅ Puzzle résolu ! Bonne séquence : 2 → 1 → 3");
        PlaySound(_successSound);
        StartCoroutine(RevealCarrotsRoutine());
    }

    /// <summary>Révèle les carottes une par une avec un effet de pop et un délai entre chaque.</summary>
    private IEnumerator RevealCarrotsRoutine()
    {
        yield return new WaitForSeconds(_rewardRevealDelay);

        foreach (GameObject carrot in _carrots)
        {
            if (carrot == null) continue;

            carrot.SetActive(true);
            StartCoroutine(PopScaleRoutine(carrot.transform));
            Debug.Log($"[BellGameManager] 🥕 Carotte apparue : {carrot.name}");
            yield return new WaitForSeconds(0.3f);
        }

        Debug.Log("[BellGameManager] 🎉 Les carottes sont disponibles !");
    }

    /// <summary>Anime le scale de 0 vers 1 avec un léger overshooting.</summary>
    private IEnumerator PopScaleRoutine(Transform target)
    {
        const float duration = 0.3f;
        const float overshoot = 1.2f;

        float elapsed = 0f;
        target.localScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = Mathf.LerpUnclamped(0f, overshoot, t < 0.7f ? t / 0.7f : 1f + (1f - t / 1f) * (overshoot - 1f));
            target.localScale = Vector3.one * Mathf.Clamp(scale, 0f, overshoot);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    /// <summary>Réinitialise la séquence du joueur.</summary>
    public void ResetSequence()
    {
        _playerSequence.Clear();
        Debug.Log("[BellGameManager] Séquence réinitialisée — Réessayez.");
    }

    private void HideCarrots()
    {
        foreach (GameObject carrot in _carrots)
        {
            if (carrot != null)
                carrot.SetActive(false);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
