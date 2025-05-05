using UnityEngine;
using UnityEngine.UIElements;

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

    private void Start()
    {
        laserRenderer.SetPosition(0, transform.position);
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, laserRange))
        {
            laserRenderer.SetPosition(1, hit.point);
            if (hit.collider.CompareTag("Player"))
                for (int i = 0; i < Physics.OverlapSphereNonAlloc(hit.point, alertRange, guardBuffer); i++)
                {
                    if (guardBuffer[i].TryGetComponent(out Guard guard))
                    {
                        if(guard.GetGuardState() == Guard.GuardState.Patrol)
                            guard.ForceAggrestion(laserRange - Vector3.Distance(hit.point, transform.position));

                        guard.UpdatePlayerLastSeen(hit.point);
                    }
                }
        }
        else
        {
            laserRenderer.SetPosition(1, transform.position + transform.forward * laserRange);
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
