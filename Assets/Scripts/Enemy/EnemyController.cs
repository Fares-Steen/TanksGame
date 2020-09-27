
using Assets.Scripts.Enemy.EnemyStates;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private string currentStateName;
    private IAiState currentState;

    public PatrolingState patrolingState = new PatrolingState();
    public AttackingState attackingState = new AttackingState();
    public ChasingState chasingState = new ChasingState();
    public FleeingState fleeingState = new FleeingState();


    public NavMeshAgent agent;

    public Transform players;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 firstPosition;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointsSet;
    public bool walkPointsAwayFromPlayerSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks = 3f;
    public bool alreadyAttacked;

    //State
    private float sightRange = 20f;
    private float attackRange = 12f;
    [HideInInspector]
    public bool playerInSightRange, playerInAttachRange;
    private TankHealth health;
    public bool LowInhealth { get; private set; }

    private void OnEnable()
    {
        currentState = patrolingState;
    }

    private void Start()
    {
        players = GameObject.FindGameObjectWithTag("Player").transform;

        firstPosition = transform.position;
        health = GetComponent<TankHealth>();
        health.TankInlowHealth += WhenEnemeyTakeDamage;
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttachRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        currentState = currentState.DoState(this);
        currentStateName = currentState.ToString();

    }










    private void WhenEnemeyTakeDamage(object sender, float currentHealth)
    {
        if (currentHealth < 50)
        {
            LowInhealth = true;
        }
        if (currentHealth <= 0)
        {
            health.TankInlowHealth -= WhenEnemeyTakeDamage;
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
