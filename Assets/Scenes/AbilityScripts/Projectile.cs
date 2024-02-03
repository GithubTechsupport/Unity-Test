using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Projectile : Abilities

{
  public GameObject wep;
  public float dmg;
  public float speed;

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
    wep.GetComponent<wepFollowBezier>().FollowTrigger();
    wep.GetComponent<wepFollowBezier>().projectileCharged = true;
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.abilityOn = true;
      }
    }
    wep.GetComponent<wepFollowBezier>().projectileSpeed = speed;
    wep.GetComponent<wepFollowBezier>().projectileDmg = dmg;
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == this) {
        comp.abilityOn = false;
      }
    }
  }

  public override void BeginCooldown(GameObject parent) {
  }

}
