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
    [SerializeField] private GameObject navpoint;
    [SerializeField] private float attackDistance = 1.5f;


    private void Update()
    {
        //Si j'ai une condition de changement d'état
        if (TestChangeState())
        {
            //alors je change d'état. 
            ChangeState();
        }
        Behaviour();
    }

    private bool TestChangeState()
    {
        switch (state)
        {
            case StateType.Attack:
                if (!GetComponent<SightPerception>().isDetected)
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
                if (GetComponent<SightPerception>().isDetected)
                {
                    if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
                    {
                        //alors j'attaque
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

                if (!GetComponent<SightPerception>().isDetected)
                {
                    nextState = StateType.Patrol;
                    return true;
                }
                // si la distance entre l'agent et le joueur est inférieur à ma distance d'attaque

                if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
                {
                    //alors j'attaque
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
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
                break;
            case StateType.Patrol:
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
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
        GetComponent<NavMeshAgent>().SetDestination(navpoint.transform.position);
        GetComponent<Animator>().SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);
    }

    private void FollowBehavior()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        GetComponent<Animator>().SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);

    }

    private void AttackBehavior()
    {
        GetComponent<Animator>().SetTrigger(name: "Punch");
    }
}