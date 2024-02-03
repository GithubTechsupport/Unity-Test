using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SlamAbility : Abilities
{

  public GameObject wep;

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
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.abilityOn = true;
      }
    }
    wep.GetComponent<SlamAbilityMono>().abilityObj = this;
    wep.GetComponent<SlamAbilityMono>().parent = parent;
    wep.GetComponent<SlamAbilityMono>().wep = wep;
    wep.GetComponent<SlamAbilityMono>().dmg = dmg;
    wep.GetComponent<SlamAbilityMono>().startAbility = true;
  }

  public override void BeginCooldown(GameObject parent) {
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<NothingPersonnelKidMono>().movePoint);
    parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepFollowBezier>().moveToBase = true;
  }
}
