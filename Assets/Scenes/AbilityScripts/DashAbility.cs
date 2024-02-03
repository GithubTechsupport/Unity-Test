using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Abilities
{
  public float dashingPower = 24f;
  private Rigidbody2D rb;
  private TrailRenderer tr;
  float originalGravity;
  Object ability;
  GameObject wep;

  public override void SetType(GameObject parent) {
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.type = AbilityHolder.AbilityType.movement;
        comp.typeSet = true;
      }
    }
  }

  public override void Activate(GameObject parent) {
    wep = parent.GetComponent<Player1Pickup>().pickedWep;
    tr = parent.GetComponent<TrailRenderer>();
    rb = parent.GetComponent<Rigidbody2D>();

    parent.GetComponent<Player1Mov>().dashing = true;
    originalGravity = rb.gravityScale;
    rb.gravityScale = 0f;
    if (wep) {
      rb.velocity = new Vector2(parent.transform.localScale.x * dashingPower * wep.GetComponent<SwordStats>().agiMult, 0f);
    } else {rb.velocity = new Vector2(parent.transform.localScale.x * dashingPower, 0f);}
    tr.emitting = true;
    //Debug.Log(parent.GetComponent<AbilityHolder>().state);
  }

  public override void BeginCooldown(GameObject parent) {
    tr = parent.GetComponent<TrailRenderer>();
    rb = parent.GetComponent<Rigidbody2D>();
    tr.emitting = false;
    rb.gravityScale = originalGravity;
    parent.GetComponent<Player1Mov>().dashing = false;
  }
}
