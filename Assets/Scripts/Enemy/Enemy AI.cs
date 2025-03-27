using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Redemptiom.Utils;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] State staringState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaming
    }

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;
        state = staringState;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                break;

        }
    }

    private void Roaming()
    {
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}
