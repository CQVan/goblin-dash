using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [System.Serializable]
    public struct PatrolPoint
    {
        public Transform location;
        [Tooltip("Time guard idles at location after arrival")]
        public float idleDuration;
    }

    public enum GuardState
    {
        Patrol,
        Chase
    }

    [Header("References")]
    [SerializeField] private GuardSettings settings;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private LayerMask guardLayer;

    [Header("Patrol Path")]
    [SerializeField] private PatrolPoint[] patrolPath;

    private GameObject player;
    private NavMeshAgent agent;

    private Vector3 playerLastSeen;
    private GuardState state = GuardState.Patrol;

    private Coroutine patrolCoroutine;
    private int patrolIndex = 0;

    private float currentAggressionDistance = 0.0f;
    private Coroutine deaggressionCoroutine = null;
    private bool canDeaggress;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        agent.speed = settings.guardPatrolSpeed;
        patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }

    private void Update()
    {
        switch (state)
        {
            case GuardState.Patrol:
                PatrolLoop();
                break;

            case GuardState.Chase:
                ChaseLoop();
                break;

            default:
                Debug.LogWarning("GuardState not implemented");
                break;
        }
    }

    private void ChaseLoop()
    {

        if (CanSeePlayer())
        {
            agent.SetDestination(playerLastSeen);

            // Cancel any deaggression coroutine
            canDeaggress = false;
            if (deaggressionCoroutine != null)
            {
                StopCoroutine(deaggressionCoroutine);
                deaggressionCoroutine = null;
            }

            // todo add contagious guard aggression
        }
        else if(HasAgentReachedLocation())
        {
            if (deaggressionCoroutine == null)
                deaggressionCoroutine = StartCoroutine(DeaggressionCoroutine());

            if (canDeaggress)
            {
                currentAggressionDistance -= settings.deaggressionRate * Time.deltaTime;
            }

            if (currentAggressionDistance <= 0)
            {
                currentAggressionDistance = 0;

                // return guard to patrol state
                state = GuardState.Patrol;
                agent.speed = settings.guardPatrolSpeed;
                patrolCoroutine = StartCoroutine(PatrolCoroutine());
                return;
            }
        }
    }

    private void PatrolLoop()
    {
        if (CanSeePlayer())
        {
            // Cancel any deaggression coroutine
            canDeaggress = false;
            if(deaggressionCoroutine != null)
            {
                StopCoroutine(deaggressionCoroutine);
                deaggressionCoroutine = null;
            }

            currentAggressionDistance += (false ? settings.sneakingDetectionRate : settings.detectionRate) * Time.deltaTime;
            Debug.Log(currentAggressionDistance);
        }
        else
        {
            if (deaggressionCoroutine == null)
                deaggressionCoroutine = StartCoroutine(DeaggressionCoroutine());

            if (canDeaggress)
            {
                currentAggressionDistance -= settings.deaggressionRate * Time.deltaTime;
                currentAggressionDistance = Mathf.Max(currentAggressionDistance, 0);
            }
        }

        // Check if guard fully detects player
        float distanceToPlayer = Vector3.Distance(eyeTransform.position, player.transform.position);
        if (currentAggressionDistance > distanceToPlayer)
        {
            // Convert guard to chase state
            state = GuardState.Chase;
            StopCoroutine(patrolCoroutine);
            agent.speed = settings.guardChaseSpeed;
            return;
        }
    }

    private IEnumerator DeaggressionCoroutine()
    {
        yield return new WaitForSeconds(settings.startDeaggressionTime);
        deaggressionCoroutine = null;
        canDeaggress = true;
    }

    private IEnumerator PatrolCoroutine()
    {
        agent.SetDestination(patrolPath[patrolIndex].location.position);

        // Wait until the patrol point has been reached
        while (!HasAgentReachedLocation())
        {
            yield return new WaitForEndOfFrame();
        }

        // Idle guard
        yield return new WaitForSeconds(patrolPath[patrolIndex].idleDuration);

        // Start next patrol coroutine
        patrolIndex++;

        if (patrolIndex >= patrolPath.Length)
        {
            patrolIndex = 0;
        }

        patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }

    #region utilities
    private bool CanSeePlayer(bool writeToLastSeenLocation = true)
    {
        float angleToPlayer = Vector3.Angle(eyeTransform.forward, player.transform.position - transform.position);
        float distanceToPlayer = Vector3.Distance(eyeTransform.position, player.transform.position);

        float detectionDistance = false ? settings.sneakingDetectionDistance : settings.detectionStartDistance;

        if (detectionDistance > distanceToPlayer && Mathf.Abs(angleToPlayer) < settings.detectionAngle / 2.0f)
        {
            if(writeToLastSeenLocation)
                playerLastSeen = player.transform.position;
            return true;
        }
        
        return false;
    }

    private bool HasAgentReachedLocation()
    {
        return Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance;
    }

    private void OnDrawGizmos()
    {
        float detectionDistance = false ? settings.sneakingDetectionDistance : settings.detectionStartDistance;

        Ray leftRay = new Ray(eyeTransform.position, Quaternion.AngleAxis(-settings.detectionAngle/2, transform.up) * eyeTransform.forward);
        Ray rightRay = new Ray(eyeTransform.position, Quaternion.AngleAxis(settings.detectionAngle / 2, transform.up) * eyeTransform.forward);
        Vector3 leftEnd = leftRay.GetPoint(detectionDistance);
        Vector3 rightEnd = rightRay.GetPoint(detectionDistance);

        Gizmos.DrawLine(eyeTransform.position, leftEnd);
        Gizmos.DrawLine(eyeTransform.position, rightEnd);


        if (player != null)
        {
            if (CanSeePlayer(false))
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
    #endregion
}
