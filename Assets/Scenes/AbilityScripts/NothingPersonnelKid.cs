using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NothingPersonnelKid : Abilities
{

  public GameObject wep;

  public int dir;

  public float dmg;

  public int dist;

  public float hbxHeight;

  public float slowSpeed;

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
      comp.allOff = true;
      if (comp.ability == this) {
        comp.abilityOn = true;
      }
    }
    wep.GetComponent<NothingPersonnelKidMono>().abilityObj = this;
    wep.GetComponent<NothingPersonnelKidMono>().parent = parent;
    wep.GetComponent<NothingPersonnelKidMono>().wep = wep;
    wep.GetComponent<NothingPersonnelKidMono>().hbxh = hbxHeight;
    wep.GetComponent<NothingPersonnelKidMono>().dist = dist;
    wep.GetComponent<NothingPersonnelKidMono>().dmg = dmg;
    wep.GetComponent<NothingPersonnelKidMono>().startAbility = true;
    wep.GetComponent<NothingPersonnelKidMono>().newVel = slowSpeed;
  }

  public override void BeginCooldown(GameObject parent) {
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<NothingPersonnelKidMono>().movePoint);
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<NothingPersonnelKidMono>().movePoint2);
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<NothingPersonnelKidMono>().boxPoint);
    Destroy(parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<NothingPersonnelKidMono>().boxPoint2);
    parent.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepFollowBezier>().moveToBase = true;
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      comp.allOff = false;
    }
  }
}
