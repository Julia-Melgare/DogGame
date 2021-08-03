﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 15;
    public Color meshColor = Color.yellow;
    public int scanFrequency = 30;

    private Mesh mesh;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }
}