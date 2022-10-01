using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterLegs : MonoBehaviour
{
    [SerializeField] private float speed;
    [Header("Jump")]
    [SerializeField] private float jumpAscendTime;
    [SerializeField] private float jumpFallTime;
    [SerializeField] private float jumpFloatTime;
    [SerializeField] private float jumpHeight;
    private bool grounded = true;
    private bool intentionToJump;
    private float jumpTimer;

    private Rigidbody2D rb;
    private Collider2D coll;

    [HideInInspector] public float driveX;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(driveX * speed, rb.velocity.y);
        HandleJump();
    }

    public void SetJump(bool val) {
        if (intentionToJump && !val) {
            var velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;
        }
        intentionToJump = val;
    }
    
    private void HandleJump() {

        grounded = CheckGrounded();

        if (grounded) {
            jumpTimer = 1;
        }

        if (intentionToJump && jumpTimer > 0) {
            var velocity = rb.velocity;
            velocity.y = 2 * jumpHeight / jumpAscendTime;
            jumpTimer = 0;
            rb.velocity = velocity;
        }

        if (!grounded) {
            var gravity = -2 * jumpHeight / (jumpAscendTime * jumpAscendTime);
            
            if (rb.velocity.y < 0) {
                if (intentionToJump) {
                    gravity = -2 * jumpHeight / (jumpFloatTime * jumpFloatTime);
                } else {
                    gravity = -2 * jumpHeight / (jumpFallTime * jumpFallTime);
                }
            }

            rb.AddForce(gravity * rb.mass * Vector2.up);
        }
    }

    private bool CheckGrounded() {
        const float window = 0.05f;
        return Physics2D.Raycast(new Vector2(coll.bounds.center.x, coll.bounds.min.y + window * 0.5f), Vector2.down, window, 1);
    }
}
