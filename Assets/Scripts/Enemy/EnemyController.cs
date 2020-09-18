
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform players;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointsSet;
    public float walkPointRange;

    //Attacking
    private float timeBetweenAttacks = 3f;
    bool alreadyAttacked;

    //State
    private float sightRange = 20f;
    private float attackRange = 12f;
    private bool playerInSightRange, playerInAttachRange;

    private void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttachRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttachRange && !playerInSightRange)
        {
            Patroling();
        }
        if (!playerInAttachRange && playerInSightRange)
        {
            ChasePlayer();
        }
        if (playerInAttachRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointsSet)
        {
            SearchWalkPoint();
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointsSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointsSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(players.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(players);

        if (!alreadyAttacked)
        {
            GetComponent<EnemyShoot>().Fire(players.position);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void SetAttackRange(float range)
    {
        attackRange = range;
    }
    public void SetSightRange(float range)
    {
        sightRange = range;
    }

    public void SetTimeBetweenAttacks(float time)
    {
        timeBetweenAttacks = time;
    }

    public void SetEnemySpeed(float speed)
    {
        agent.speed = speed;
    }

}
