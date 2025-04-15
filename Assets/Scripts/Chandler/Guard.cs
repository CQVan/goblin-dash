using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum GuardState
{
    Patrol,
    Investigate,
    Chasing
}

[RequireComponent(typeof(NavMeshAgent))]
public class Guard : MonoBehaviour
{
    [Serializable]
    public struct GuardPoint
    {
        public Transform location;
        public float idleTimeOnArrive;
    }

    [Header("References")]
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip footstep;

    [Header("Path Configs")]
    public GuardPoint[] patrolRoute;

    [Header("Attention Config")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float detectionAngle;
    [SerializeField] private float attentionRate;
    [SerializeField] private float attentionDecreaseRate;

    [SerializeField] private bool showDebug = false;

    private GameObject player;
    private NavMeshAgent agent;

    private int currentPointIndex = 0;
    private float attention = 0;
    private Vector3 playerLastSeen = new Vector3();

    private GuardState state = GuardState.Patrol;
    private Coroutine idleCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");

        if (player == null)
            Debug.LogError("No Player found!");
    }

    private void Start()
    {
        if(patrolRoute.Length > 0)
            agent.SetDestination(patrolRoute[currentPointIndex].location.position);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 0.5f <= 0.01f && (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run")))
        {
            AudioSource.PlayClipAtPoint(footstep, transform.position, 10);
        }
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case GuardState.Patrol:
                GuardPatrol();
                break;

            case GuardState.Chasing:
                ChasePlayer();
                break;

            case GuardState.Investigate:
                Investigate();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(1);
        }
    }

    private void Investigate()
    {
        throw new NotImplementedException();
    }

    private void ChasePlayer()
    {
        if (CanSeePlayer(out Vector3 location))
        {
            playerLastSeen = location;
            agent.SetDestination(playerLastSeen);
        }
        else
        {
            attention -= attentionDecreaseRate * Time.deltaTime;
            if(attention <= 0)
            {
                if (showDebug) Debug.Log("Player Lost: Returning to Patrol");
                attention = 0;
                eyeTransform.localRotation = Quaternion.Euler(0,0,0);
                state = GuardState.Patrol;
                animator.SetTrigger("Walk");
                return;
            }
        }

        eyeTransform.LookAt(playerLastSeen);
    }

    private void GuardPatrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && idleCoroutine == null && patrolRoute.Length > 0)
        {
            idleCoroutine = StartCoroutine(IdleAndMove());
        }

        bool increaseAttention = CanSeePlayer(out playerLastSeen);

        if (increaseAttention)
        {
            attention += attentionRate * Time.deltaTime;

            if (attention >= 100)
            {
                if (showDebug) Debug.Log("Player Found: Starting Chase");
                state = GuardState.Chasing;
                animator.SetTrigger("Run");
            }
        }
        else
            attention -= attentionDecreaseRate * Time.deltaTime;
    }

    private IEnumerator IdleAndMove()
    {
        animator.SetTrigger("Idle");

        yield return new WaitForSeconds(patrolRoute[currentPointIndex].idleTimeOnArrive);
        currentPointIndex++;

        if(currentPointIndex >= patrolRoute.Length)
            currentPointIndex = 0;

        animator.SetTrigger("Walk");
        agent.SetDestination(patrolRoute[currentPointIndex].location.position);
        idleCoroutine = null;

    }

    public void PlayGuardFootstep(Vector3 location)
    {

    }

    private bool CanSeePlayer(out Vector3 location)
    {

        if (Vector3.Distance(player.transform.position, eyeTransform.position) <= detectionRange)
            if (Vector3.Angle(eyeTransform.forward, player.transform.position - eyeTransform.position) <= detectionAngle)
                if (Physics.Linecast(eyeTransform.position, player.transform.position, out RaycastHit hit))
                    if (hit.transform.CompareTag("Player"))
                    {   
                        location = hit.collider.transform.position;
                        return true;
                    }

        location = Vector3.zero;
        return false;
    }

    private bool CanSeePlayer()
    {
        return CanSeePlayer(out Vector3 local);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = CanSeePlayer() ? Color.green : Color.red;
            Gizmos.DrawLine(eyeTransform.position, player.transform.position);
        }
        Ray leftRay = new Ray(eyeTransform.position, Quaternion.AngleAxis(-detectionAngle, eyeTransform.up) * eyeTransform.forward);
        Ray rightRay = new Ray(eyeTransform.position, Quaternion.AngleAxis(detectionAngle, eyeTransform.up) * eyeTransform.forward);
        Vector3 leftEnd = leftRay.GetPoint(detectionRange);
        Vector3 rightEnd = rightRay.GetPoint(detectionRange);

        Gizmos.DrawLine(eyeTransform.position, leftEnd);
        Gizmos.DrawLine(eyeTransform.position, rightEnd);
        
        if(state == GuardState.Chasing)
        {
            Gizmos.DrawWireSphere(playerLastSeen, 1);
        }
    }
}
