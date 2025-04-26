using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class laser : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask ignoreMask;
    private bool hit = false;

    private RaycastHit RaycastHit;
    private Ray ray;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        ray = new(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit, distance, ~ignoreMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1,RaycastHit.point);
            if (RaycastHit.collider.gameObject.CompareTag("Player") && !hit)
            {
                hit = true;
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            lineRenderer.SetPosition(0,transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * distance);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, ray.direction * distance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(RaycastHit.point, 0.23f);
    }
}
