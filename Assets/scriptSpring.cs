using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptSpring : MonoBehaviour
{
    private Animator animator;
    void Start() {
        animator = this.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
           Debug.Log("Collision");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            animator.SetBool("isActive", true);
            Invoke("StopAnimation", 0.3f);
        }
    }

    private void StopAnimation() {
        animator.SetBool("isActive", false);
    }
}
