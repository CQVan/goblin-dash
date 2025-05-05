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
    public GuardSettings settings;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private LayerMask guardLayer;
    [SerializeField] private GameObject aggroDecal;

    [Header("Patrol Path")]
    [SerializeField] private PatrolPoint[] patrolPath;

    private GameObject player;
    private Collider playerCollider;
    private NavMeshAgent agent;

    private static Vector3 playerLastSeen;
    private GuardState state = GuardState.Patrol;

    private Coroutine patrolCoroutine;
    private int patrolIndex = 0;

    private float currentAggressionDistance = 0.0f;
    private Coroutine deaggressionCoroutine = null;
    private bool canDeaggress;
    private bool preventDegress = false;

    private bool forcedAggression = false;
    [HideInInspector] public float joinChaseTimer = 0;
    private Coroutine joinChaseTimerResetCoroutine = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<Collider>();
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

    private void LateUpdate()
    {
        if (aggroDecal.activeSelf)
            aggroDecal.transform.LookAt(Camera.main.transform.position);
    }

    private Collider[] guardsNearby = new Collider[32];
    private void ChaseLoop()
    {

        if (CanSeePlayer())
        {

            // Cancel any deaggression coroutine
            canDeaggress = false;
            if (deaggressionCoroutine != null)
            {
                StopCoroutine(deaggressionCoroutine);
                deaggressionCoroutine = null;
            }
            else
                currentAggressionDistance = Mathf.Min(currentAggressionDistance + settings.detectionRate * Time.deltaTime, settings.detectionStartDistance);

            int guards = Physics.OverlapCapsuleNonAlloc(transform.position, transform.position + Vector3.up, settings.joinChaseMaxDistance, guardsNearby, guardLayer);

            for(int i = 0; i < guards; i++)
            {
                // check if collider is a guard
                if (!guardsNearby[i].TryGetComponent<Guard>(out var guard)) continue;

                // prevent guard from aggroing himself
                if (guard == this) continue;

                Debug.DrawLine(transform.position, guard.transform.position);

                // check if guard can see other guard
                if (!Physics.Linecast(transform.position, guard.transform.position)) continue;

                guard.IncrementChaseJoinTimer(currentAggressionDistance);

                //TODO start chase
                forcedAggression = true;
            }
        }
        else if (HasAgentReachedLocation())
        {
            // Start deaggression timer if not started
            if (deaggressionCoroutine == null)
                deaggressionCoroutine = StartCoroutine(DeaggressionCoroutine());

            // Check if timer finished
            if (canDeaggress || preventDegress)
            {
                currentAggressionDistance -= settings.deaggressionRate * Time.deltaTime;
            }

            // Deaggro if run out of aggression distance
            if (currentAggressionDistance <= 0)
            {
                currentAggressionDistance = 0;

                // return guard to patrol state
                state = GuardState.Patrol;
                agent.speed = settings.guardPatrolSpeed;
                patrolCoroutine = StartCoroutine(PatrolCoroutine());
                forcedAggression = false;
                return;
            }
        }

        agent.SetDestination(playerLastSeen);
    }

    public void ForceAggrestion(float newAggression)
    {
        aggroDecal.SetActive(true);
        agent.speed = 0;

        StartCoroutine(DelayedFunction(1, () =>
        {
            aggroDecal.SetActive(false);
            agent.speed = settings.guardChaseSpeed;
        }));

        currentAggressionDistance = newAggression;

        Debug.Log("aggression forced: " + gameObject.name, this);

        // Convert guard to chase state
        state = GuardState.Chase;
        if (patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
    }

    public GuardState GetGuardState() { return state; }

    public void UpdatePlayerLastSeen(Vector3 location)
    {
        playerLastSeen = location;
    }

    public IEnumerator PreventDegress(float duration)
    {
        preventDegress = true;
        yield return new WaitForSeconds(duration);
        preventDegress = false;
    }

    public void IncrementChaseJoinTimer(float newAggression)
    {
        if (forcedAggression) return;

        joinChaseTimer += Time.deltaTime;

        joinChaseTimerResetCoroutine ??= StartCoroutine(JoinChaseResetCoroutine());

        if (joinChaseTimer < settings.timeToJoinChase) return;

        forcedAggression = true;
        ForceAggrestion(newAggression);

        if (joinChaseTimerResetCoroutine != null)
        {
            StopCoroutine(joinChaseTimerResetCoroutine);
            joinChaseTimerResetCoroutine = null;
        }
        joinChaseTimer = 0;
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
            //Debug.Log(currentAggressionDistance);
        }
        else
        {
            if (deaggressionCoroutine == null)
                deaggressionCoroutine = StartCoroutine(DeaggressionCoroutine());

            if (canDeaggress || preventDegress)
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
            ForceAggrestion(currentAggressionDistance);
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
        if (patrolPath.Length == 0) yield break;

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

    private IEnumerator JoinChaseResetCoroutine()
    {
        yield return new WaitForSeconds(settings.timeToResetJoinChase);
        joinChaseTimer = 0;
    }

    #region utilities

    public float GetCurrentAggroDistance() { return currentAggressionDistance; }

    private bool CanSeePlayer(bool writeToLastSeenLocation = true)
    {
        float angleToPlayer = Vector3.Angle(eyeTransform.forward, player.transform.position - transform.position);
        float distanceToPlayer = Vector3.Distance(eyeTransform.position, player.transform.position);

        float detectionDistance = false ? settings.sneakingDetectionDistance : settings.detectionStartDistance;

        Debug.Log($"{detectionDistance > distanceToPlayer} && {Mathf.Abs(angleToPlayer) < settings.detectionAngle / 2.0f}");

        if (detectionDistance > distanceToPlayer && Mathf.Abs(angleToPlayer) < settings.detectionAngle / 2.0f)
        {
            if (Physics.Linecast(transform.position, playerCollider.bounds.center, out RaycastHit hit, ~guardLayer))
                if (!hit.collider.CompareTag("Player"))
                    return false;

            if(writeToLastSeenLocation)
                playerLastSeen = player.transform.position;
            return true;
        }
        
        return false;
    }

    private IEnumerator DelayedFunction(float delay, System.Action func)
    {
        yield return new WaitForSeconds(delay);
        func.Invoke();
    }

    private bool HasAgentReachedLocation()
    {
        return Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance || agent.pathStatus == NavMeshPathStatus.PathPartial;
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

            Gizmos.DrawLine(transform.position, playerCollider.bounds.center);
        }
    }
    #endregion
}
