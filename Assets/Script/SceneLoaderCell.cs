using UnityEngine;

public class SceneLoaderCell : MonoBehaviour, IActionable
{
    [Header("Scene Settings")]
    [SerializeField] private string _sceneToLoad = "NextLevel";

    [Header("Turn Settings")]
    [SerializeField] private TurnManager _turnManager;

    [Header("UI Settings")]
    [SerializeField] private LevelCompleteUI _levelCompleteUI;

    [Header("Celebration Settings")]
    [SerializeField] private ParticleSystem _confettiEffect;

    private bool _hasCompleted = false;

    public void Action(Player CurrentPawn)
    {
        if (_hasCompleted)
        {
            return;
        }

        if (_turnManager != null)
        {
            _turnManager.IncrementTurn();

            if (_turnManager.HasCompletedAllTurns())
            {
                _hasCompleted = true;
                Debug.Log("Tous les tours complétés ! Affichage du panneau de victoire...");

                if (_confettiEffect != null)
                {
                    _confettiEffect.Play();
                }

                if (_levelCompleteUI != null)
                {
                    _levelCompleteUI.ShowLevelComplete(_sceneToLoad);
                }
            }
        }
    }
}
