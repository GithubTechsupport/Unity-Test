using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AbilityHolder : MonoBehaviour
{
  public Abilities ability;
  PhotonView view;
  float cooldownTime;
  float activeTime;
  public bool abilityOn;
  public bool typeSet;
  public bool allOff;

  private GameObject wep;
  public enum AbilityState {
    ready,
    active,
    cooldown
  }
  public enum AbilityType {
    movement,
    sword,
    spell
  }
  public AbilityState state = AbilityState.ready;
  public AbilityType type = AbilityType.movement;

  public KeyCode key;

  void Start() {
    view = gameObject.GetComponent<PhotonView>();
    abilityOn = false;
    typeSet = false;
    allOff = false;
  }

  void Update()
  {
    if (view.IsMine) {


    wep = gameObject.GetComponent<Player1Pickup>().pickedWep;
    if (!typeSet && ability != null) {
      ability.SetType(gameObject);
    }
    switch (type) {
      case AbilityType.movement:
      switch (state) {
        case AbilityState.ready:
        if (!gameObject.GetComponent<PlayerStats>().dead && !allOff) {
          if (Input.GetKeyDown(key)) {
            ability.Activate(gameObject);
            state = AbilityState.active;
            activeTime = ability.activeTime;
          }
        }
        break;

        case AbilityState.active:
        if (activeTime > 0) {
          activeTime -= Time.deltaTime;

        } else {
          ability.BeginCooldown(gameObject);
          state = AbilityState.cooldown;
          if (wep) {
            cooldownTime = ability.cooldownTime * wep.GetComponent<SwordStats>().cdMult;
          } else {cooldownTime = ability.cooldownTime;}
        }
        break;

        case AbilityState.cooldown:
        if (cooldownTime > 0) {
          cooldownTime -= Time.deltaTime;

        } else {
          state = AbilityState.ready;
        }
        break;
      }
      break;

      case AbilityType.sword:
      switch (state) {
        case AbilityState.ready:
        if (!allOff && !gameObject.GetComponent<PlayerStats>().dead && gameObject.GetComponent<Player1Pickup>().pickedWep && gameObject.GetComponent<Player1Pickup>().pickedWep.GetComponent<wepFollowBezier>().resetPos) {
          if (Input.GetKeyDown(key)) {
            ability.Activate(gameObject);
            state = AbilityState.active;
          }
        }
        break;

        case AbilityState.active:
        if (abilityOn) {
          return;

        }
        ability.BeginCooldown(gameObject);
        state = AbilityState.cooldown;
        if (wep) {
          cooldownTime = ability.cooldownTime * wep.GetComponent<SwordStats>().cdMult;
        } else {cooldownTime = ability.cooldownTime;}

        break;

        case AbilityState.cooldown:
        if (cooldownTime > 0) {
          cooldownTime -= Time.deltaTime;

        } else {
          state = AbilityState.ready;
        }
        break;
      }
      break;

    }
}
  }
}
