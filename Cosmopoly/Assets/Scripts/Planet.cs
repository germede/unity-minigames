using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet {

	public GameObject gameObject;
	public List<Terrain> terrains;
	
	public Planet(GameObject gameObject, List<Triangle> triangles) {
		this.gameObject = gameObject;
        this.terrains = new List<Terrain>();
        for (int i = 0; i < triangles.Count; i++) {

            terrains.Add(new Terrain(triangles[i]));

        }

	}

	public void ChangeTerrain(Vector3 hitPoint, int terrainIndex, float tolerance) {

		Vector3[] vertices = gameObject.GetComponent<MeshFilter> ().mesh.vertices;
		Vector3[] normals = gameObject.GetComponent<MeshFilter> ().mesh.normals;
		Color32[] colors32 = gameObject.GetComponent<MeshFilter> ().mesh.colors32;

		for (int i = 0; i < terrains.Count; i++)
		{
			Triangle triangle = terrains[i].triangle;

			if (triangle.IsPointInside (hitPoint, tolerance)) {

				triangle.Extrude (Terrain.elevations[terrainIndex]);

				// VERTICES
				vertices[i * 3 + 0] = triangle.a;
				vertices[i * 3 + 1] = triangle.b;
				vertices[i * 3 + 2] = triangle.c;

				// NORMALS
				normals[i * 3 + 0] = triangle.a;
				normals[i * 3 + 1] = triangle.b;
				normals[i * 3 + 2] = triangle.c;

				// COLOR
				colors32[i * 3 + 0] = Terrain.colors[terrainIndex];
				colors32[i * 3 + 1] = Terrain.colors[terrainIndex];
				colors32[i * 3 + 2] = Terrain.colors[terrainIndex];

				terrains [i].SetIndex(terrainIndex);

			}
		}

		gameObject.GetComponent<MeshFilter> ().mesh.vertices = vertices;
		gameObject.GetComponent<MeshFilter> ().mesh.normals = normals;
		gameObject.GetComponent<MeshFilter> ().mesh.colors32 = colors32;

	}



	public void PlaceResource(Vector3 hitPoint, GameObject resourcePrefab, float tolerance) {

		foreach (Terrain terrain in terrains)
		{
			var triangle = terrain.triangle;
			if (triangle.IsPointInside (hitPoint, tolerance)) {
				terrain.PlaceResource (hitPoint, resourcePrefab);
			}
		}
	}
		
}
