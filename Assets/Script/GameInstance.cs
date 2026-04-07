using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton persistant entre les scènes.
/// Mémorise l'état de progression de chaque DialogueComponent
/// afin de le restaurer au rechargement de la Dev_map.
/// </summary>
public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance { get; private set; }

    // Dialogues ayant terminé leur premier échange (cases du board débloquées).
    private readonly HashSet<string> _firstCompletedIds = new HashSet<string>();

    // Dialogues ayant donné leur récompense (état final, quête terminée).
    private readonly HashSet<string> _rewardGivenIds = new HashSet<string>();

    // Cases qui ont déjà chargé leur scène et ne doivent plus être activables.
    private readonly HashSet<string> _loadedSceneIds = new HashSet<string>();

    // Cases dont l'action de retour (désactivation de cases) a déjà été exécutée.
    private readonly HashSet<string> _returnActionIds = new HashSet<string>();

    // Nom de la case sur laquelle le joueur doit apparaître au prochain chargement.
    private string _spawnCellName = null;

    // Noms des cases à réactiver au prochain chargement de Dev_map (usage unique).
    private string[] _cellNamesToActivate = null;

    // Nombre de carottes rapportées depuis un mini-jeu.
    private int _carrotsFromMiniGame = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Enregistre qu'un dialogue a terminé son premier échange (ActivateFirstObjects vient d'être appelé).</summary>
    public void RegisterFirstDialogueCompleted(string dialogueId)
    {
        _firstCompletedIds.Add(dialogueId);
        Debug.Log($"[GameInstance] Premier dialogue terminé : {dialogueId}");
    }

    /// <summary>Enregistre qu'un dialogue a donné sa récompense (état final).</summary>
    public void RegisterRewardGiven(string dialogueId)
    {
        _firstCompletedIds.Add(dialogueId);
        _rewardGivenIds.Add(dialogueId);
        Debug.Log($"[GameInstance] Récompense donnée : {dialogueId}");
    }

    /// <summary>Retourne vrai si le premier dialogue de cet ID a été complété.</summary>
    public bool IsFirstDialogueCompleted(string dialogueId) => _firstCompletedIds.Contains(dialogueId);

    /// <summary>Retourne vrai si la récompense de cet ID a été donnée (état final).</summary>
    public bool IsRewardGiven(string dialogueId) => _rewardGivenIds.Contains(dialogueId);

    /// <summary>Enregistre qu'une CellSceneLoader a déjà chargé sa scène.</summary>
    public void RegisterSceneLoaded(string cellId)
    {
        _loadedSceneIds.Add(cellId);
        Debug.Log($"[GameInstance] Scene chargée enregistrée : {cellId}");
    }

    /// <summary>Retourne vrai si cette CellSceneLoader a déjà chargé sa scène.</summary>
    public bool IsSceneAlreadyLoaded(string cellId) => _loadedSceneIds.Contains(cellId);

    /// <summary>Enregistre que l'action de retour de cette case a été exécutée.</summary>
    public void RegisterReturnAction(string cellId) => _returnActionIds.Add(cellId);

    /// <summary>Retourne vrai si l'action de retour de cette case a déjà été exécutée.</summary>
    public bool IsReturnActionDone(string cellId) => _returnActionIds.Contains(cellId);

    /// <summary>Définit la case de spawn pour le prochain chargement de scène.</summary>
    public void SetSpawnCell(string cellName) => _spawnCellName = cellName;

    /// <summary>Retourne le nom de la case de spawn et l'efface (usage unique).</summary>
    public string GetAndClearSpawnCell()
    {
        string name = _spawnCellName;
        _spawnCellName = null;
        return name;
    }

    /// <summary>Retourne vrai si un spawn override est défini.</summary>
    public bool HasSpawnOverride() => !string.IsNullOrEmpty(_spawnCellName);

    /// <summary>Enregistre les noms des cases à réactiver au prochain chargement.</summary>
    public void SetCellsToActivateOnReturn(string[] cellNames) => _cellNamesToActivate = cellNames;

    /// <summary>Retourne vrai si des cases doivent être réactivées.</summary>
    public bool HasCellsToActivate() => _cellNamesToActivate != null && _cellNamesToActivate.Length > 0;

    /// <summary>Retourne les noms des cases à réactiver et efface la liste (usage unique).</summary>
    public string[] GetAndClearCellsToActivate()
    {
        string[] names = _cellNamesToActivate;
        _cellNamesToActivate = null;
        return names;
    }

    /// <summary>Enregistre le nombre de carottes rapportées depuis le mini-jeu.</summary>
    public void SetCarrotsFromMiniGame(int count) => _carrotsFromMiniGame = count;

    /// <summary>Retourne le nombre de carottes rapportées.</summary>
    public int GetCarrotsFromMiniGame() => _carrotsFromMiniGame;

    /// <summary>Remet le compteur de carottes à zéro.</summary>
    public void ClearCarrotsFromMiniGame() => _carrotsFromMiniGame = 0;

    /// <summary>Retourne vrai si des carottes ont été rapportées depuis le mini-jeu.</summary>
    public bool HasCarrotsFromMiniGame() => _carrotsFromMiniGame > 0;
}
