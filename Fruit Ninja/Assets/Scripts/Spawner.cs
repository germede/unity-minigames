using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] objsToSpawn;
    public Transform[] spawnPlaces;
    public float minWait = .3f;
    public float maxWait = 1f;

    private void Start() {
        StartCoroutine(SpawnFruits());
    }

    private IEnumerator SpawnFruits() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            Transform t = spawnPlaces[Random.Range(0, spawnPlaces.Length)];


            GameObject go = null;
            float p = Random.Range(0, 100);

            if(p < 10)
                go = objsToSpawn[0];
            else
                go = objsToSpawn[Random.Range(1, objsToSpawn.Length)];

            GameObject fruit = Instantiate(go, t.position, t.rotation);

            fruit.GetComponent<Rigidbody2D>().AddForce(t.transform.up * Random.Range(12,17), ForceMode2D.Impulse);

            Debug.Log("Fruit gets spawned");
            Destroy(fruit, 5);
        }
    }

}
