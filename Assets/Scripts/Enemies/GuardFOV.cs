using System.Collections.Generic;
using UnityEngine;

public class GuardFOV : MonoBehaviour
{
    [Header("Mesh Settings")]
    [SerializeField] private int meshResolution;
    [SerializeField] private int edgeResolveIterations;
    [SerializeField] private float edgeDstThreshold;
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private MeshFilter aggroMeshFilter;
    [SerializeField] private LayerMask ignoreMask;
    Mesh regularMesh;
    Mesh aggroMesh;

    private GuardSettings settings;
    private Guard guard;
    private void Awake()
    {
        guard = GetComponent<Guard>();
        settings = guard.settings;
    }

    private void Start()
    {
        regularMesh = new()
        {
            name = "Normal View Mesh"
        };

        aggroMesh = new()
        {
            name = "Aggro View Mesh"
        };

        viewMeshFilter.mesh = regularMesh;
        aggroMeshFilter.mesh = aggroMesh;
    }

    private void LateUpdate()
    {
        if (guard.GetCurrentAggroDistance() != settings.detectionStartDistance)
        {
            viewMeshFilter.gameObject.SetActive(true);
            DrawFieldOfView(regularMesh, settings.detectionStartDistance);
        }else
            viewMeshFilter.gameObject.SetActive(false);


        if (guard.GetCurrentAggroDistance() > 0)
        {
            aggroMeshFilter.gameObject.SetActive(true);
            DrawFieldOfView(aggroMesh, guard.GetCurrentAggroDistance());
        }
        else
            aggroMeshFilter.gameObject.SetActive(false);
            
    }

    void DrawFieldOfView(Mesh viewMesh, float range)
    {
        int stepCount = Mathf.RoundToInt(settings.detectionAngle * meshResolution);
        float stepAngleSize = settings.detectionAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - settings.detectionAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle, range);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast, range);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, float range)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle, range);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle, float distance)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, distance, ~ignoreMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * distance, distance, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
