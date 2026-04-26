using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public enum StateType
{
    None,
    Patrol,
    Follow,
    Attack
}

public class LH_IAController : MonoBehaviour
{

    [SerializeField] private StateType state = StateType.None;
    [SerializeField] private StateType nextState = StateType.None;
    [SerializeField] private GameObject target;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Patrouille aléatoire")]
    [SerializeField] private float _wanderRadius = 15f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private SightPerception _sight;

    private bool _hasWanderDestination = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _sight = GetComponent<SightPerception>();
    }

    /// <summary>Remet l'IA en état Patrol depuis un état arrêté (ex: après désactivation).</summary>
    public void ResetToPatrol()
    {
        _hasWanderDestination = false;
        state = StateType.Patrol;
        nextState = StateType.None;
    }

    private void Update()
    {
        //Si j'ai une condition de changement d'�tat
        if (TestChangeState())
        {
            //alors je change d'�tat. 
            ChangeState();
        }
        Behaviour();
    }

    private bool TestChangeState()
    {
        switch (state)
        {
            case StateType.Attack:
                if (!_sight.isDetected)
                {
                    nextState = StateType.Patrol;
                    return true;
                }

                if (Vector3.Distance(target.transform.position, transform.position) > attackDistance)
                {
                    nextState = StateType.Follow;
                    return true;
                }
                break;

            case StateType.Patrol:
                if (_sight.isDetected)
                {
                    if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
                    {
                        nextState = StateType.Attack;
                        return true;
                    }
                    else
                    {
                        nextState = StateType.Follow;
                        return true;
                    }
                }
                break;

            case StateType.Follow:
                if (!_sight.isDetected)
                {
                    nextState = StateType.Patrol;
                    return true;
                }

                if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
                {
                    nextState = StateType.Attack;
                    return true;
                }
                break;
        }
        return false;
    }

    private void ChangeState()
    {
        EndState();
        state = nextState;
        StartState();
    }

    private void StartState()
    {

    }

    private void EndState()
    {
        switch (state)
        {
            case StateType.Follow:
            case StateType.Patrol:
                _agent.ResetPath();
                _hasWanderDestination = false;
                break;
        }
    }


    private void Behaviour()
    {
        switch (state)
        {
            case StateType.Patrol:
                PatrolBehavior();
                break;
            case StateType.Follow:
                FollowBehavior();
                break;
            case StateType.Attack:
                AttackBehavior();
                break;
        }
    }

    private void PatrolBehavior()
    {
        // Tire un nouveau point aléatoire quand l'IA n'a pas de destination ou qu'elle est arrivée
        bool destinationReached = !_agent.pathPending
            && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f;

        if (!_hasWanderDestination || destinationReached)
            _hasWanderDestination = TrySetRandomDestination();

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    /// <summary>Echantillonne un point aléatoire sur le NavMesh et l'assigne comme destination.</summary>
    private bool TrySetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
            return true;
        }
        return false;
    }

    private void FollowBehavior()
    {
        _agent.SetDestination(target.transform.position);
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void AttackBehavior()
    {
        _animator.SetTrigger(name: "Punch");
    }
}