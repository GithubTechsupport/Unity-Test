using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAbility2Mono : MonoBehaviour
{

  public GameObject parent;
  public GameObject wep;

  public Object abilityObj;

  public bool startAbility;
  public bool key_checker;
  public bool key_pressed;

  private Rigidbody2D rb;

  public float RotateSpeed;
  public float Radius;
  public float dashSpeed;
  public float dashInc;
  public float dmg;
  private float dashTime;
  float originalGravity;

  private Vector2 _centre;
  private Vector2 offset;
  private float _angle;

  public KeyCode key;

  private IEnumerator circle() {
    wep.GetComponent<wepFollowBezier>().resetPos = false;
    wep.transform.parent = null;
    rb = parent.GetComponent<Rigidbody2D>();
    originalGravity = rb.gravityScale;
    startAbility = false;
    while (!key_pressed) {
      _centre = parent.transform.position;
      _angle += RotateSpeed * Time.deltaTime;

      offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
      wep.transform.position = _centre + offset;
      wep.transform.up = ((Vector2)parent.transform.position - (Vector2)wep.transform.position ).normalized;
      yield return new WaitForEndOfFrame();
    }
    if (key_pressed) {
      key_pressed = false;
      rb.gravityScale = 0f;
      rb.velocity = new Vector2(0,0);
      parent.GetComponent<Player1Mov>().dashing = true;
      foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
        comp.allOff = true;
      }
      parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepAttack>().enableAttack = true;
      wep.GetComponent<wepAttack>().damage = dmg;
      while (dashTime <= 1) {

        _centre = parent.transform.position;
        dashTime += Time.deltaTime * dashSpeed;
        wep.transform.parent = parent.transform;
        rb.velocity += (_centre - new Vector2(wep.transform.position.x,wep.transform.position.y)) * Time.deltaTime * -dashInc * wep.GetComponent<SwordStats>().spdMult;
        yield return new WaitForEndOfFrame();
      }
      if (dashTime >= 1) {
        foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
          if (comp.ability == abilityObj) {
            comp.abilityOn = false;
          }
        }
        rb.velocity = new Vector2(0f,0f);
        parent.GetComponent<Player1Mov>().dashing = false;
        rb.gravityScale = originalGravity;
        wep.transform.parent = null;
        dashTime = 0;
      }
    }
  }

  private void Start() {
    startAbility = false;
    key_checker = false;
    key_pressed = false;
    dashTime = 0;
  }

  private void Update()
  {
    if (key_checker) {
      if (Input.GetKeyDown(key)) {
        key_pressed = true;
        key_checker = false;
      }
    }
    if (startAbility) {
      StartCoroutine(circle());
      key_checker = true;
    }
  }

}
