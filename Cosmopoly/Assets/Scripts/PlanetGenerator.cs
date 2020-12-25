using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetGenerator : MonoBehaviour
{

	private List<Triangle> triangles;

	public int subdivisions = 3;
    public Material material;
	Planet planet;

	public GameObject haloPrefab;
	public GameObject shadowSpherePrefab;

    public void Start()
    {
        InitializeIcosahedron();
		SubdivideIcosahedron();
        GeneratePlanet();
    }

    public void InitializeIcosahedron() {
        List<Vector3> vertices = new List<Vector3>();
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
        vertices.Add(new Vector3(-1,  t,  0).normalized);
        vertices.Add(new Vector3( 1,  t,  0).normalized);
        vertices.Add(new Vector3(-1, -t,  0).normalized);
        vertices.Add(new Vector3( 1, -t,  0).normalized);
        vertices.Add(new Vector3( 0, -1,  t).normalized);
        vertices.Add(new Vector3( 0,  1,  t).normalized);
        vertices.Add(new Vector3( 0, -1, -t).normalized);
        vertices.Add(new Vector3( 0,  1, -t).normalized);
        vertices.Add(new Vector3( t,  0, -1).normalized);
        vertices.Add(new Vector3( t,  0,  1).normalized);
        vertices.Add(new Vector3(-t,  0, -1).normalized);
        vertices.Add(new Vector3(-t,  0,  1).normalized);

		triangles = new List<Triangle>();
		triangles.Add(new Triangle( vertices[0], vertices[11],  vertices[5]));
		triangles.Add(new Triangle( vertices[0],  vertices[5],  vertices[1]));
		triangles.Add(new Triangle( vertices[0],  vertices[1],  vertices[7]));
		triangles.Add(new Triangle( vertices[0],  vertices[7], vertices[10]));
		triangles.Add(new Triangle( vertices[0], vertices[10], vertices[11]));
		triangles.Add(new Triangle( vertices[1],  vertices[5],  vertices[9]));
		triangles.Add(new Triangle( vertices[5], vertices[11],  vertices[4]));
		triangles.Add(new Triangle(vertices[11], vertices[10],  vertices[2]));
		triangles.Add(new Triangle(vertices[10],  vertices[7],  vertices[6]));
		triangles.Add(new Triangle( vertices[7],  vertices[1],  vertices[8]));
		triangles.Add(new Triangle( vertices[3],  vertices[9],  vertices[4]));
		triangles.Add(new Triangle( vertices[3],  vertices[4],  vertices[2]));
		triangles.Add(new Triangle( vertices[3],  vertices[2],  vertices[6]));
		triangles.Add(new Triangle( vertices[3],  vertices[6],  vertices[8]));
		triangles.Add(new Triangle( vertices[3],  vertices[8],  vertices[9]));
		triangles.Add(new Triangle( vertices[4],  vertices[9],  vertices[5]));
		triangles.Add(new Triangle( vertices[2],  vertices[4], vertices[11]));
		triangles.Add(new Triangle( vertices[6],  vertices[2], vertices[10]));
		triangles.Add(new Triangle( vertices[8],  vertices[6],  vertices[7]));
		triangles.Add(new Triangle( vertices[9],  vertices[8],  vertices[1]));
    }

    public void SubdivideIcosahedron() {

		for (int i = 0; i < subdivisions; i++)
        {
            List<Triangle> newTriangles = new List<Triangle>();
            foreach (Triangle triangle in triangles)
            {
                Vector3 ab = Vector3.Lerp(triangle.a, triangle.b, 0.5f).normalized;
                Vector3 bc = Vector3.Lerp(triangle.b, triangle.c, 0.5f).normalized;
                Vector3 ca = Vector3.Lerp(triangle.c, triangle.a, 0.5f).normalized;

				newTriangles.Add(new Triangle(triangle.a, ab, ca));
				newTriangles.Add(new Triangle(triangle.b, bc, ab));
				newTriangles.Add(new Triangle(triangle.c, ca, bc));
				newTriangles.Add(new Triangle(ab, bc, ca));
            }

            triangles = newTriangles;
        }
    }

    public void GeneratePlanet()
    {
		
		int vertexCount = triangles.Count * 3;
        int[] indices = new int[vertexCount];
        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals  = new Vector3[vertexCount];
        Color32[] colors32   = new Color32[vertexCount];

        for (int i = 0; i < triangles.Count; i++)
        {
			var triangle = triangles[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

			vertices[i * 3 + 0] = triangle.a;
			vertices[i * 3 + 1] = triangle.b;
			vertices[i * 3 + 2] = triangle.c;

			normals[i * 3 + 0] = triangle.a;
            normals[i * 3 + 1] = triangle.b;
            normals[i * 3 + 2] = triangle.c;

			colors32[i * 3 + 0] = new Color32(0,0,139,255);
			colors32[i * 3 + 1] = new Color32(0,0,139,255);
			colors32[i * 3 + 2] = new Color32(0,0,139,255);

        }

		planet = new Planet(new GameObject("Planet"), triangles);
		if (FindObjectOfType<PlanetEditor> ()) {
			FindObjectOfType<PlanetEditor> ().planet = planet;
		}
		GameObject halo = Instantiate(haloPrefab) as GameObject;
		halo.transform.SetParent (planet.gameObject.transform, false);

		MeshRenderer surfaceRenderer = planet.gameObject.AddComponent<MeshRenderer>();
        surfaceRenderer.material     = material;

		MeshFilter terrainFilter = planet.gameObject.AddComponent<MeshFilter>();
        Mesh terrainMesh = new Mesh();
        terrainMesh.vertices = vertices;
        terrainMesh.normals  = normals;
        terrainMesh.colors32 = colors32;
        terrainMesh.SetTriangles(indices, 0);
        terrainFilter.mesh = terrainMesh;

		SphereCollider sphereCollider = planet.gameObject.AddComponent<SphereCollider> ();
        sphereCollider.radius = 1;

		if (shadowSpherePrefab) {
			GameObject shadowSphere = Instantiate (shadowSpherePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			shadowSphere.transform.localScale += new Vector3 (
				triangles [0].a.magnitude - 0.02f * subdivisions, 
				triangles [0].b.magnitude - 0.02f * subdivisions, 
				triangles [0].c.magnitude - 0.02f * subdivisions);
		}

    }


}
