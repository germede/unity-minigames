using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetEditor : MonoBehaviour {

	public Planet planet;

	public bool paintEnabled = true;

	public Button buttonPrefab;
	private Button lastSelectedButton;

	public float brushSize = 0;
	public Slider brushSizeSlider;

	public Transform terrainPanel;
	private int terrainIndex = -1;

	public GameObject[] resourcePrefabs;
	public Transform resourcesPanel;
	private int resourcesIndex = -1;

	void Start() {

		brushSizeSlider.onValueChanged.AddListener(delegate {brushSize = brushSizeSlider.value;});

		for (int i = 0; i < Terrain.names.Length; i++) {
			Button terrainButton = (Button)Instantiate(buttonPrefab,terrainPanel);
			int j = i;
			terrainButton.onClick.AddListener(delegate {SelectTerrain(j, terrainButton);});
			Vector3 pos = terrainButton.transform.position;
			pos.y -= 45f * i;
			terrainButton.transform.position = pos;
			terrainButton.transform.GetChild(0).GetComponentInChildren<Image>().color = Terrain.colors[i];
			terrainButton.transform.GetChild(1).GetComponentInChildren<Text> ().text = Terrain.names [i];
		}

		for (int i = 0; i < resourcePrefabs.Length; i++) {
			Button resourceButton = (Button)Instantiate(buttonPrefab,resourcesPanel);
			int j = i;
			resourceButton.onClick.AddListener(delegate {SelectResource(j, resourceButton);});
			Vector3 pos = resourceButton.transform.position;
			pos.y -= 45f * i;
			resourceButton.transform.position = pos;

			Texture2D tex = RuntimePreviewGenerator.GenerateModelPreview(resourcePrefabs[i].transform);
			Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

			resourceButton.transform.GetChild (0).GetComponentInChildren<Image> ().sprite = sprite;
			resourceButton.transform.GetChild(1).GetComponentInChildren<Text>().text = Resource.names[i];
		}
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit) && paintEnabled) {
				if (terrainIndex > -1) {
					planet.ChangeTerrain (hit.point, terrainIndex, brushSize);
				} else if (resourcesIndex > -1) {
					planet.PlaceResource (hit.point, resourcePrefabs [resourcesIndex], brushSize);
				}
				paintEnabled = false;
				Invoke("reEnablePaint",0.01f);
			}
		}
	}

	public void reEnablePaint() {
		paintEnabled = true;
	}

	public void SelectTerrain(int terrainIndex, Button terrainButton) {
		this.terrainIndex = terrainIndex;
		resourcesIndex = -1;
		terrainButton.GetComponent<Image>().color = new Color32(200,200,200,255);
		if (lastSelectedButton) {
			lastSelectedButton.GetComponent<Image>().color = new Color32(255,255,255,255);
		}

		if (terrainButton == lastSelectedButton) {
			lastSelectedButton.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
			this.terrainIndex = -1;
			lastSelectedButton = null;
		} else {
			lastSelectedButton = terrainButton;
		}
	}

	public void SelectResource(int resourceIndex, Button resourceButton) {
		this.resourcesIndex = resourceIndex;
		terrainIndex = -1;
		resourceButton.GetComponent<Image>().color = new Color32(200,200,200,255);
		if (lastSelectedButton || resourceButton == lastSelectedButton) {
			lastSelectedButton.GetComponent<Image>().color = new Color32(255,255,255,255);
		}

		if (resourceButton == lastSelectedButton) {
			lastSelectedButton.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
			this.resourcesIndex = -1;
			lastSelectedButton = null;
		} else {
			lastSelectedButton = resourceButton;
		}	
	}
}
