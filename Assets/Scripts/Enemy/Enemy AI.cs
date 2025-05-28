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
    [SerializeField] private float maxDistanceFromStart = 15f; 

    [Header("Combat Settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 7f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private LayerMask obstacleLayers;

    private NavMeshAgent navMeshAgent;
    private Transform player;
    private float roamingTimer;
    private float lastAttackTime;
    private EnemyHealth enemyHealth;
    private Vector3 startPosition; 
    private PlayerHealth playerHealth;

    [Header("Звуки")]
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip enemyShootingSound;
    private AudioSource movementAudioSource; 
    private AudioSource shootingAudioSource; 


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        startPosition = transform.position;

        movementAudioSource = gameObject.AddComponent<AudioSource>();
        shootingAudioSource = gameObject.AddComponent<AudioSource>();
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
        if (playerHealth != null && playerHealth.CurrentHealth <= 0)
        {
            currentState = State.Roaming;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = distanceToPlayer <= detectionRadius && HasLineOfSightToPlayer();

        // Убрали проверку расстояния до стартовой позиции для реактивности
        if (canSeePlayer)
        {
            currentState = distanceToPlayer <= attackRadius
                ? State.Attack
                : State.Chase;
        }
        else
        {
            // Возвращаемся только если ушли слишком далеко
            float distanceFromStart = Vector3.Distance(transform.position, startPosition);
            currentState = distanceFromStart > maxDistanceFromStart
                ? State.Roaming
                : State.Roaming;

            if (distanceFromStart > maxDistanceFromStart)
            {
                navMeshAgent.SetDestination(startPosition);
            }
        }
    }

    private bool HasLineOfSightToPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Игнорируем коллайдеры врагов и игрока
        int layerMask = obstacleLayers;
        layerMask &= ~(1 << LayerMask.NameToLayer("Enemy")); // Игнорируем слой врагов
        layerMask &= ~(1 << LayerMask.NameToLayer("Player")); // Игнорируем слой игрока

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer.normalized,
            distanceToPlayer,
            layerMask
        );

        // Отладочная визуализация
        Debug.DrawLine(
            transform.position,
            hit.collider ? (Vector3)hit.point : player.position,
            hit.collider ? Color.red : Color.green,
            0.1f
        );

        return hit.collider == null;
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
        float distanceFromStart = Vector3.Distance(transform.position, startPosition);
        navMeshAgent.SetDestination(player.position);
        if (distanceFromStart <= maxDistanceFromStart)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.SetDestination(startPosition);
        }
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
        if (playerHealth != null && playerHealth.CurrentHealth <= 0) return;

        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);

        shootingAudioSource.PlayOneShot(enemyShootingSound);
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(roamingDistanceMin, roamingDistanceMax);
        Vector3 potentialPosition = startPosition + randomDirection;

        if (Vector3.Distance(potentialPosition, startPosition) > maxDistanceFromStart)
        {
            potentialPosition = startPosition + (potentialPosition - startPosition).normalized * maxDistanceFromStart * 0.9f;
        }

        return potentialPosition;
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
        if (!animator || !movementAudioSource) return;

        float speedNormalized = Mathf.Clamp01(navMeshAgent.velocity.magnitude / navMeshAgent.speed);
        animator.SetFloat("Speed", speedNormalized);

        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            if (!movementAudioSource.isPlaying) 
            {
                movementAudioSource.clip = footstepSound;
                movementAudioSource.Play(); 
            }
        }
        else
        {
            movementAudioSource.Stop(); 
        }
    }
}