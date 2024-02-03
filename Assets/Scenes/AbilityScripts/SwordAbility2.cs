using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SwordAbility2 : Abilities
{

  [SerializeField] private float RotateSpeed = 5f;
  [SerializeField] private float Radius = 0.1f;
  [SerializeField] private float dashSpeed;
  [SerializeField] private float dashInc;
  public float dmg;

  public GameObject wep;

  public int dir;

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
    wep.GetComponent<SwordAbility2Mono>().dmg = dmg * wep.GetComponent<SwordStats>().dmgMult;
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.abilityOn = true;
        wep.GetComponent<SwordAbility2Mono>().key = comp.key;
      }
    }
    wep.GetComponent<SwordAbility2Mono>().abilityObj = this;
    wep.GetComponent<SwordAbility2Mono>().parent = parent;
    wep.GetComponent<SwordAbility2Mono>().wep = wep;
    wep.GetComponent<SwordAbility2Mono>().RotateSpeed = RotateSpeed;
    wep.GetComponent<SwordAbility2Mono>().Radius = Radius;
    wep.GetComponent<SwordAbility2Mono>().dashSpeed = dashSpeed;
    wep.GetComponent<SwordAbility2Mono>().dashInc = dashInc;
    wep.GetComponent<SwordAbility2Mono>().startAbility = true;
  }

  public override void BeginCooldown(GameObject parent) {
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      comp.allOff = false;
    }
    parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepAttack>().enableAttack = false;
    parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepFollowBezier>().moveToBase = true;
  }
}
