using UnityEngine;

public class Fruit : MonoBehaviour {

    public GameObject slicedFruitPrefab;

    public void CreateSlicedFruit() {
        // Instantiate a sliced fruit
        GameObject inst = Instantiate(slicedFruitPrefab, transform.position, transform.rotation);

        Rigidbody[] rbsOnSliced = inst.transform.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody r in rbsOnSliced) {
            r.transform.rotation = Random.rotation;
            r.AddExplosionForce(Random.Range(500, 1000), transform.position, 5);
        }

        // Get score
        FindObjectOfType<GameManager>().IncreaseScore(2);

        Destroy(inst.gameObject, 5);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Blade b = collision.GetComponent<Blade>();

        if(!b)
            return;

        CreateSlicedFruit();
    }
}
