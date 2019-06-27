using UnityEngine;

public class Paddle : MonoBehaviour {

    public float speed = 10;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        float v = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(0,v);
    }
}
