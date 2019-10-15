using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPlayer : MonoBehaviour
{
    private float velocity;
    public GameObject foot;
    private float jumpForce;
    private float flyingForce;
    public LayerMask ground;
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private bool isJumping;
    public bool isDying;
    private bool isIdle;

    private void Start() {
        isIdle = false;
        isDying = false;
        velocity = 5f;
        jumpForce = 12f;
        flyingForce = -0.7f;
        animator = this.GetComponent<Animator>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        CheckDeath();
        CheckFallInPit();
        if (!isIdle) {
            MovePlayer();
            Jump();
            ReloadIsJumping();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy") && !isDying) {
            ChangeVerticalVelocity(2*jumpForce /3);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Spring")) {
            SpringJump(other);
            EnableIdle(0.5f);
        }
    }

    private void MovePlayer() {
        float x = Input.GetAxis("Horizontal");
        ChangeHorizontalVelocity(x == 0 ? rigidbody2D.velocity.x : x * velocity);
        SetRunningAnimation(x != 0);
        FlipSprite(x);
    }

    private void Jump() {
        if (ValidJump()) {
            ChangeVerticalVelocity(jumpForce);
            StartJumpAnimation();
        } else if (ValidGlide()) { 
            ChangeVerticalVelocity(flyingForce); 
            StartGlideAnimation();
        } else {
            StartIdleAnimation();
        }
    }

    private void SpringJump(Collider2D other) {
        rigidbody2D.velocity = 2*jumpForce* other.gameObject.transform.up;
        StartJumpAnimation();
    }
    private void CheckFallInPit() {
        if (this.transform.position.y < -3f) {
            animator.SetBool("isDying", true);
        }
    }

    private void SetRunningAnimation(bool isRunning) {
        animator.SetBool("isRunning", isRunning);
    }

    private void CheckDeath() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RaumanDeath")) {
            isDying = true;
            isIdle = true;
            animator.SetBool("void", true);
        } else if (isDying){
            FindObjectOfType<scriptGM>().EndGame();
        }
    }

    private void FlipSprite(float side) {
        if (side < 0) 
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        if (side > 0)
            this.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ReloadIsJumping() {
        Collider2D collider2D = GetFootCollider();
        if (collider2D == null) {
            animator.SetBool("isFalling", true);
            isJumping = true;
        } else {
            isJumping = false;
            animator.SetBool("isFalling", false);
        }
    }

    private void ChangeHorizontalVelocity(float hVelocity) {
        rigidbody2D.velocity = new Vector2(hVelocity, 
                rigidbody2D.velocity.y);
    }

    private void ChangeVerticalVelocity(float vVelocity) {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 
                vVelocity);
    }

    private void StartIdleAnimation() {
        animator.SetBool("isPlanning", false);
        animator.SetBool("isJumping", false);
    }
    private void StartJumpAnimation() {
        animator.SetBool("isPlanning", false);
        animator.SetBool("isJumping", true);
    }

    private void StartGlideAnimation() {
        animator.SetBool("isPlanning", true);
    }

    private void EnableIdle(float waitTime) {
        isIdle = true;
        Invoke("DisableIdle", waitTime);
    }
    private void DisableIdle() {
        isIdle = false;
    }

    private Collider2D GetFootCollider() {
        return Physics2D.OverlapCircle(
            foot.transform.position, 0.1f, ground);
    }

    private bool ValidJump() {
        return Input.GetKeyDown(KeyCode.Space) && !isJumping;
    }

    private bool ValidGlide() {
        return Input.GetKey(KeyCode.Space) && isJumping &&
             rigidbody2D.velocity.y < 0;
    }
}
