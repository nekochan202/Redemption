using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform enemyVisual; // ���������� ����� (������)
    [SerializeField] private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 5f;
    [SerializeField] private float rotationSpeed = 15f; // �������� ��������
    [SerializeField] private float movementThreshold = 0.1f;

    [Header("Health System")]
    [SerializeField] private EnemyHealth enemyHealth;

    private NavMeshAgent navMeshAgent;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        if (!enemyVisual) enemyVisual = transform.Find("EnemyVisual");
        if (!animator && enemyVisual) animator = enemyVisual.GetComponent<Animator>();

        if (!animator) Debug.LogError("Animator not found!", this);
        if (!enemyVisual) Debug.LogError("EnemyVisual not assigned!", this);

        if (!enemyHealth) enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        UpdateAnimation();
        HandleMovement();
        HandleRotation();

        if (enemyHealth != null && enemyHealth.IsDead()) return;
    }

    private void UpdateAnimation()
    {
        if (!animator || !navMeshAgent) return;

        float speedNormalized = Mathf.Clamp01(navMeshAgent.velocity.magnitude / navMeshAgent.speed);
        animator.SetFloat("Speed", speedNormalized);
    }

    private void HandleMovement()
    {
        roamingTime -= Time.deltaTime;
        if (roamingTime < 0)
        {
            Roaming();
            roamingTime = roamingTimerMax;
        }
    }

    private void HandleRotation()
    {
        if (!enemyVisual || navMeshAgent.velocity.magnitude < movementThreshold) return;

        // �������� ����������� ��������
        Vector2 moveDirection = navMeshAgent.velocity.normalized;

        // ��������� ���� �������� � ��������
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // ������� ���������� ��� ��������
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ������ ������������ ���������� �����
        enemyVisual.rotation = Quaternion.Slerp(
            enemyVisual.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0).normalized * Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}