using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserRange;
    public float alertRange;

    private Collider[] guardBuffer = new Collider[32];
    private LineRenderer laserRenderer;

    private void Awake()
    {
        laserRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, laserRange))
        {
            laserRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
            if (hit.collider.CompareTag("Player"))
                for (int i = 0; i < Physics.OverlapSphereNonAlloc(hit.point, alertRange, guardBuffer); i++)
                {
                    if (guardBuffer[i].TryGetComponent(out Guard guard))
                    {
                        if(guard.GetGuardState() == Guard.GuardState.Patrol)
                        {
                            guard.UpdatePlayerLastSeen(hit.point);
                            guard.ForceAggrestion(laserRange - Vector3.Distance(hit.point, transform.position));
                        }
                    }
                }
        }
        else
        {
            laserRenderer.SetPosition(1, Vector3.forward * laserRange);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * laserRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, alertRange);    
    }
}
