using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptRabbit : MonoBehaviour
{
    public GameObject head;
    public LayerMask player;
    private float velocity;
    private int hDirection;
    private Rigidbody2D rigidbody2D;
    private bool isDead;
    private Animator animator;

    private void Start() {
        velocity = 1f;
        hDirection = -1;
        animator = this.GetComponent<Animator>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        isDead = false;
    }

    private void Update() {
        Walk();
        FlipSprite(hDirection);
        CheckIfIsDead();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && 
                !other.gameObject.GetComponent<scriptPlayer>().isDying &&
                !isDead) {
            Collider2D collider2D = Physics2D.OverlapCircle(
                head.transform.position, 0.3f, player);
            if (collider2D == null) {
                KillPlayer(other);
            }
            else {
                KillYourself();
            }
        }
        else {
            ChangeDirection();
        }
    }

    private void CheckIfIsDead() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RabbitDeath")) {
            isDead = true;
        } else if (isDead){
            Destroy(this.gameObject);
        }
    }

    private void Walk() {
        rigidbody2D.velocity = new Vector2(hDirection * velocity, 
                                    rigidbody2D.velocity.y);
        animator.SetBool("isWalking", true);
    }

    private void KillYourself() {
        animator.SetBool("IsDying", true);
        Invoke("DesactivateColliderAndRigidbody", 0.3f);
        hDirection = 0;
    }

    private void KillPlayer(Collision2D other) {
        other.gameObject.GetComponent<Animator>().SetBool("isDying", true);
    }

    private void ChangeDirection() {
            hDirection *= -1;
    }
    private void FlipSprite(float side) {
        if (side < 0) 
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        if (side > 0)
            this.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void DesactivateColliderAndRigidbody() {
        rigidbody2D.gravityScale = 0;
        this.GetComponent<CapsuleCollider2D>().enabled = false;
    }
    
}
