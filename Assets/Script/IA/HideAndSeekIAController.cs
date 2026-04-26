using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Contrôle l'activation et la désactivation de l'IA pour le cache-cache.
/// En phase Hiding : NavMeshAgent, LH_IAController et SightPerception sont gelés.
/// En phase Seeking : tout est réactivé, et on notifie le GameManager si le joueur est détecté.
/// </summary>
[RequireComponent(typeof(LH_IAController))]
[RequireComponent(typeof(SightPerception))]
[RequireComponent(typeof(NavMeshAgent))]
public class HideAndSeekIAController : MonoBehaviour
{
    private LH_IAController _iaController;
    private SightPerception _sightPerception;
    private NavMeshAgent _navMeshAgent;

    private bool _isActive = false;

    private void Awake()
    {
        _iaController = GetComponent<LH_IAController>();
        _sightPerception = GetComponent<SightPerception>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Commence inactive, le GameManager appellera SetActive(false) au démarrage
        ApplyActiveState(false);
    }

    private void Update()
    {
        if (!_isActive) return;

        // Surveille si l'IA voit le joueur pendant la phase Seeking
        if (_sightPerception.isDetected)
        {
            HideAndSeekGameManager.Instance?.OnPlayerFound();
        }
    }

    /// <summary>Active ou désactive complètement l'IA.</summary>
    public void SetActive(bool active)
    {
        _isActive = active;
        ApplyActiveState(active);

        if (active)
            _iaController.ResetToPatrol();
    }

    private void ApplyActiveState(bool active)
    {
        _iaController.enabled = active;
        _sightPerception.enabled = active;
        _navMeshAgent.isStopped = !active;

        if (!active)
        {
            // Stoppe le NavMeshAgent sur place proprement
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
        }
    }
}
