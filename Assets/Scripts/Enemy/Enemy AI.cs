using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour {
    public enum State { Idle, Roaming, Chase, Attack }
    [SerializeField] private State currentState = State.Idle;

    [Header("References")]
    [SerializeField] private Transform enemyVisual;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Movement Settings")]
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float movementThreshold = 0.1f;

    [Header("Combat Settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 7f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float bulletSpeed = 15f;

    private NavMeshAgent navMeshAgent;
    private Transform player;
    private float roamingTimer;
    private float lastAttackTime;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (enemyHealth.IsDead()) return;

        UpdateState();
        UpdateAnimation();

        switch (currentState)
        {
            case State.Roaming:
                HandleRoaming();
                break;
            case State.Chase:
                ChasePlayer();
                break;
            case State.Attack:
                AttackPlayer();
                break;
        }

        HandleRotation();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Roaming;
        }
    }

    private void HandleRoaming()
    {
        roamingTimer -= Time.deltaTime;
        if (roamingTimer <= 0)
        {
            Vector3 roamPosition = GetRoamingPosition();
            navMeshAgent.SetDestination(roamPosition);
            roamingTimer = roamingTimerMax;
        }
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        navMeshAgent.SetDestination(transform.position);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }

    private void Shoot()
    {
        // Создать пулю и задать направление
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);
    }

    private Vector3 GetRoamingPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0
        ).normalized * Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void HandleRotation()
    {
        if (!enemyVisual) return;

        if (currentState == State.Chase || currentState == State.Attack)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enemyVisual.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (navMeshAgent.velocity.magnitude > movementThreshold)
        {
            Vector2 moveDirection = navMeshAgent.velocity.normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            enemyVisual.rotation = Quaternion.Slerp(
                enemyVisual.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void UpdateAnimation()
    {
        if (!animator) return;
        float speedNormalized = Mathf.Clamp01(navMeshAgent.velocity.magnitude / navMeshAgent.speed);
        animator.SetFloat("Speed", speedNormalized);
    }
}