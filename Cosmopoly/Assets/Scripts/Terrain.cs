using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain {

	public static string[] names = {
		"Mountain",
		"Desert",
		"Land",
		"Snow",
		"Ocean"
	};
	public static Color32[] colors = {
		new Color32(128, 64, 0, 255),	
		new Color32(255, 212, 128, 255),
		new Color32(20,  255, 30, 255),	
		new Color32(255,  255, 255, 255),
		new Color32(0,0,139,255),			
	};
	public static float[] elevations = { 0.01f, 0.005f, 0.005f, 0.005f, 0.0f };

	private int index = 4;
	public Triangle triangle;
	public List<GameObject> resources = new List<GameObject>();
	private int maxResources = 1;


	public Terrain(Triangle triangle) {
		this.triangle = triangle;
	}

	public void PlaceResource(Vector3 place, GameObject resourcePrefab) {
		if (index != 4 && resources.Count < maxResources) {
			Quaternion rotation = Quaternion.LookRotation (triangle.a - triangle.b, triangle.GetCenterPoint ());

			GameObject resource = MonoBehaviour.Instantiate (resourcePrefab, triangle.GetCenterPoint(), rotation);
			resource.transform.localScale += new Vector3 (
				Random.Range (-0.1f, 0.1f), 
				Random.Range (-0.4f, 0.4f), 
				Random.Range (-0.1f, 0.1f)
			); 
			resources.Add (resource);
		}
	}

	public void SetIndex(int index) {
		this.index = index;

		if (index == 4) {
			foreach (GameObject resource in resources) {
				GameObject.Destroy (resource);
			}
			resources.Clear ();
		}
	}
		
}