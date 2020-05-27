using UnityEngine;
using System.Collections;

public class CircleMeshCreator
{
    private static readonly int PRECISION = 1000;
    private float _radius;
    private int _segments;
    private float _innerRadius;
    private float _angleDegree;

    private Mesh _cacheMesh;

    public Mesh CreateMesh(float radius, int segments, float innerRadius, float angleDegree)
    {
        if (checkDiff(radius, segments, innerRadius, angleDegree))
        {
            Mesh newMesh = Create(radius, segments, innerRadius, angleDegree);
            if (newMesh != null)
            {
                _cacheMesh = newMesh;
                this._radius = radius;
                this._segments = segments;
                this._innerRadius = innerRadius;
                this._angleDegree = angleDegree;
            }
        }
        return _cacheMesh;
    }

    private Mesh Create(float radius, int segments, float innerRadius, float angleDegree)
    {

        if (segments <= 0)
        {
            segments = 1;
#if UNITY_EDITOR
            Debug.Log("segments must be larger than zero.");
#endif
        }

        Mesh mesh = new Mesh();
        int vlen = segments * 2 + 2;
        Vector3[] vertices = new Vector3[vlen];

        float angle = Mathf.Deg2Rad * angleDegree;
        float currAngle = angle / 2;
        float deltaAngle = angle / segments;
        for (int i = 0; i < vlen; i += 2)
        {
            float cosA = Mathf.Cos(currAngle);
            float sinA = Mathf.Sin(currAngle);
            vertices[i] = new Vector3(cosA * innerRadius, 0, sinA * innerRadius);
            vertices[i + 1] = new Vector3(cosA * radius, 0, sinA * radius);
            currAngle -= deltaAngle;
        }

        int tlen = segments * 6;
        int[] triangles = new int[tlen];
        for (int i = 0, vi = 0; i < tlen; i += 6, vi += 2)
        {
            triangles[i] = vi;
            triangles[i + 1] = vi + 1;
            triangles[i + 2] = vi + 3;
            triangles[i + 3] = vi + 3;
            triangles[i + 4] = vi + 2;
            triangles[i + 5] = vi;
        }


        Vector2[] uvs = new Vector2[vlen];
        for (int i = 0; i < vlen; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }

    private bool checkDiff(float radius, int segments, float innerRadius, float angleDegree)
    {
        return segments != this._segments || (int)((angleDegree - this._angleDegree) * PRECISION) != 0 ||
            (int)((radius - this._radius) * PRECISION) != 0 || (int)((innerRadius - this._innerRadius) * PRECISION) != 0;
    }
}