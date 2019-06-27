using UnityEngine;

public class Ball : MonoBehaviour {

    public float speed = 10;

    public Collider2D paddleLeft;
    public Collider2D paddleRight;
    public Collider2D goalLeft;
    public Collider2D goalRight;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitialBallImpulse() {
        rb.velocity = transform.right * speed;
    }

    public void ResetBall() {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        GameManager.gameRunning = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // Ball collision with paddle
        if(collision.collider == paddleLeft || collision.collider == paddleRight) {
            float y = transform.position.y - collision.transform.position.y;
            Debug.Log(y);

            float x = 0;

            if(collision.collider == paddleLeft)
                x = 1;
            else
                x = -1;

            Vector2 dir = new Vector2(x, y).normalized;
            rb.velocity = dir * speed;

            collision.transform.GetComponent<AudioSource>().Play();
        }

        // Ball collision with goals
        if(collision.collider == goalLeft || collision.collider == goalRight) {

            if(collision.collider == goalRight)
                FindObjectOfType<GameManager>().IncreaseScore(true);
            else
                FindObjectOfType<GameManager>().IncreaseScore(false);

            ResetBall();
        }
    }
}
