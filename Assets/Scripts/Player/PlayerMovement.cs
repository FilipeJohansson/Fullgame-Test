using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Public variables
    public CharacterController2D controller;
    public Animator animator;
    
    // Private variables
    private float horizontalMove = 0f;
    private bool jump = false;

    // Update is called once per frame
    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * controller.runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) {
            animator.SetBool("IsJumping", true);
            jump = true;
        }
    }

    public void onLanding() {
        animator.SetBool("IsJumping", false);
        Debug.Log("landed");
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
