using UnityEngine;

public class Guard : MonoBehaviour
{

    public enum GuardState
    {
        Patrol,
        Chase
    }

    [Header("References")]
    public GuardSettings settings;
    public Transform eyeTransform;

    private GameObject player;
    private Vector3 playerLastSeen;
    private GuardState state = GuardState.Patrol;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (state)
        {
            case GuardState.Patrol:
                PatrolLoop();
                break;

            case GuardState.Chase:

                break;

            default:
                Debug.LogWarning("GuardState not implemented");
                break;
        }
    }
    private void PatrolLoop()
    {
        if (CanSeePlayer())
            Debug.Log(CanSeePlayer());
    }

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
}
