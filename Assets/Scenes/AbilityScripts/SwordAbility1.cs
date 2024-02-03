using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SwordAbility1 : Abilities
{

  public GameObject wep;

  public int dir;

  public float dmg;

  public override void SetType(GameObject parent) {
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.type = AbilityHolder.AbilityType.sword;
        comp.typeSet = true;
      }
    }
  }

  public override void Activate(GameObject parent) {
    wep = parent.GetComponent<Player1Pickup>().pickedWep;
    wep.GetComponent<SwordAbility1Mono>().dmg = dmg * wep.GetComponent<SwordStats>().dmgMult;
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.abilityOn = true;
      }
    }
    wep.GetComponent<SwordAbility1Mono>().abilityObj = this;
    wep.GetComponent<SwordAbility1Mono>().parent = parent;
    wep.GetComponent<SwordAbility1Mono>().wep = wep;
    wep.GetComponent<SwordAbility1Mono>().startAbility = true;
  }

  public override void BeginCooldown(GameObject parent) {
    parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepAttack>().enableAttack = false;
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<SwordAbility1Mono>().movePoint);
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<SwordAbility1Mono>().movePoint2);
    parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepFollowBezier>().moveToBase = true;
  }
}
