using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 6;
    public Color meshColor = Color.yellow;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> objects = new List<GameObject>();

    private Collider[] colliders = new Collider[50];
    private Mesh mesh;
    private int count;
    private float scanInterval;
    private float scanTimer;


    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        objects.Clear();
        for (int i = 0; i <count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (isInSight(obj))
            {
                objects.Add(obj);
            }
        }
    }

    public bool isInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 dir = dest - origin;

        if (dir.y < 0 || dir.y > height) return false;

        dir.y = 0;
        float deltaAngle = Vector3.Angle(dir, transform.forward);
        if (deltaAngle > angle) return false;

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers)) return false;

        return true;
    }

    public int Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        foreach (var obj in objects)
        {
            if (obj.layer == layer) buffer[count++] = obj;
            if (count == buffer.Length) break;
        }
        return count;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        int[] triangles = new int[numVertices];
        Vector3[] vertices = new Vector3[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int v = 0;

        //left side
        vertices[v++] = bottomCenter;
        vertices[v++] = bottomLeft;
        vertices[v++] = topLeft;

        vertices[v++] = topLeft;
        vertices[v++] = topCenter;
        vertices[v++] = bottomCenter;

        //right side
        vertices[v++] = bottomCenter;
        vertices[v++] = topCenter;
        vertices[v++] = topRight;

        vertices[v++] = topRight;
        vertices[v++] = bottomRight;
        vertices[v++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i){
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            //far side
            vertices[v++] = bottomLeft;
            vertices[v++] = bottomRight;
            vertices[v++] = topRight;

            vertices[v++] = topRight;
            vertices[v++] = topLeft;
            vertices[v++] = bottomLeft;

            //top 
            vertices[v++] = topCenter;
            vertices[v++] = topLeft;
            vertices[v++] = topRight;

            //bottom
            vertices[v++] = bottomCenter;
            vertices[v++] = bottomRight;
            vertices[v++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++) triangles[i] = i;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
