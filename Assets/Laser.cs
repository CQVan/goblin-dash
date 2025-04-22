using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserRange;
    public float alertRange;

    private Collider[] guardBuffer = new Collider[32];

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, laserRange))
            if (hit.collider.CompareTag("Player"))
                for(int i = 0; i < Physics.OverlapSphereNonAlloc(hit.point, alertRange, guardBuffer); i++)
                {
                    if (guardBuffer[i].TryGetComponent<Guard>(out Guard guard))
                    {
                        guard.UpdatePlayerLastSeen(hit.point);
                        guard.ForceAggrestion(laserRange - Vector3.Distance(hit.point, transform.position));
                    }
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
