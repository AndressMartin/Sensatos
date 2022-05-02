using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private Vector3 origin;
    private float startingAngle;
    private float fov;
    private float viewDistance;

    private Enemy pai;
    public Mesh GetMesh => mesh;

    public void SetPai(Enemy enemy)
    {
        pai = enemy;
    }

    void Start()
    {
        mesh = new Mesh();

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Default";
        meshRenderer.sortingOrder = -1;

        GetComponent<MeshFilter>().mesh = mesh;

        origin = Vector3.zero;

    }

    private void LateUpdate()
    {
        if(pai != null)
        {
            meshRenderer.enabled = pai.gameObject.activeSelf;

            if(pai.gameObject.activeSelf == false)
            {
                return;
            }
        }

        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;


        //print(origin);

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                // Hit object
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }
    
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetDirection(Vector3 direction)
    {
        startingAngle =  GetAngleFromVectorFloat(direction) - fov/2f ;
    }

    public void SetArea(float fov, float viewDistance)
    {
        this.fov = fov;
        this.viewDistance = viewDistance;
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector2(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }
    float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

}
