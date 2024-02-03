using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player1Mov : MonoBehaviour
{

  PhotonView view;

  [SerializeField] public float velocityP1 = 3;
  [SerializeField] public float runSpeedModifier = 1.5f;
  [SerializeField] public LayerMask groundLayer;
  [SerializeField] public float jumpPower  = 500;
  [SerializeField] public bool moving;
  [SerializeField] public Transform groundCheckerA;
  [SerializeField] public Transform groundCheckerB;
  [SerializeField] int totalJumps;
  private int availableJumps;
  private Rigidbody2D rb;
  private Animator animator;
  public float dirValue;

  [SerializeField] public bool grounded;
  public bool facingRight = true;
  private bool running;
  public bool dashing;
  public bool stopRot;
  private bool multipleJumps;

  public KeyCode leftKey;
  public KeyCode rightKey;
  public KeyCode jumpKey;


  private void Awake()
  {
    view = GetComponent<PhotonView>();
    totalJumps = 3;
    availableJumps = totalJumps;
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    stopRot = false;
  }

  void Jump() {
    if (grounded) {
      multipleJumps = true;
      availableJumps--;
      rb.velocity = Vector2.up * jumpPower;
      animator.SetBool("Jump", true);
    } else {
      if (multipleJumps && availableJumps > 0) {
        availableJumps--;
        rb.velocity = Vector2.up * jumpPower;
        animator.SetBool("Jump", true);
      }
    }
  }

  void Move(float dir)
  {

    float xVel = dirValue * velocityP1 * 100 * Time.fixedDeltaTime;
    if (running) {
      xVel *= runSpeedModifier;
    }
    Vector2 targetVelocity = new Vector2(xVel, rb.velocity.y);
    rb.velocity = targetVelocity;
    if (facingRight && dirValue < 0)
    {
      if (gameObject.GetComponent<Player1Pickup>().pickedWep != null && !stopRot) {
        gameObject.GetComponent<Player1Pickup>().pickedWep.transform.parent = null;
      }
      transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
      facingRight = false;
    }
    else if (!facingRight && dirValue > 0)
    {
      if (gameObject.GetComponent<Player1Pickup>().pickedWep != null && !stopRot) {
        gameObject.GetComponent<Player1Pickup>().pickedWep.transform.parent = null;
      }
      transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
      facingRight = true;
    }
    animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

    if (rb.velocity.x != 0) {
      moving = true;
    } else {moving = false;}
  }

  void GroundCheck() {
    bool wasGrounded = grounded;
    Collider2D[] colliders = Physics2D.OverlapAreaAll(groundCheckerA.position, groundCheckerB.position, groundLayer);
    if (colliders.Length > 0) {
      grounded = true;
      if (!wasGrounded) {
        availableJumps = totalJumps;
      }
    } else {
      grounded = false;
    }
    animator.SetBool("Jump", !grounded);
  }

  private void FixedUpdate() {
    if (view.IsMine){
    if (gameObject.GetComponent<PlayerStats>().knockback) {
      return;
    }
    if (dashing) {
      return;
    }
    Move(dirValue);
    GroundCheck();
  }
  }

  private float doubleTapTime = 0.5f;
  private float _lastDashButtonTime;

  private void Update() {
    if (view.IsMine) {
    if (gameObject.GetComponent<PlayerStats>().dead) {
      return;
    }

    if (dashing) {
      return;
    }

    if (Input.GetKey(leftKey)) {
      dirValue = -1;
    } else if (Input.GetKey(rightKey)) {
      dirValue = 1;
    } else {dirValue = 0;}

    if ((Input.GetKeyDown(leftKey)) || (Input.GetKeyDown(rightKey))) {
      if (Time.time - _lastDashButtonTime < doubleTapTime)
      running = true;
      _lastDashButtonTime = Time.time;
    } else if ((Input.GetKeyUp(leftKey)) || (Input.GetKeyUp(rightKey))) {
      running = false;
    }

    if (Input.GetKeyDown(jumpKey)) {
      Jump();
      animator.SetBool("Jump", true);
    }

    animator.SetFloat("yVelocity", rb.velocity.y);
  }
  }
}
