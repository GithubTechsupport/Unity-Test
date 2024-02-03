using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAbility1Mono : MonoBehaviour
{

  public GameObject wep;
  public GameObject movePoint;
  public GameObject movePoint2;
  public GameObject parent;

  public Object abilityObj;

  public int dir;

  public float dmg;

  public bool startAbility;

  void Start()
  {
    startAbility = false;
  }

  void Update()
  {
    if (startAbility) {
      StartCoroutine(ability());
    }
  }

  private IEnumerator ability() {
    startAbility = false;
    movePoint = new GameObject("movePoint");
    movePoint2 = new GameObject("movePoint2");
    movePoint.transform.parent = parent.transform;
    movePoint2.transform.parent = parent.transform;
    movePoint.transform.localPosition = new Vector3(-1.75f,2.5f,0);
    movePoint2.transform.localPosition = new Vector3(7,0,0);
    movePoint.transform.parent = null;
    movePoint2.transform.parent = null;

    wep = parent.GetComponent<Player1Pickup>().pickedWep;
    if (parent.GetComponent<Player1Mov>().facingRight) {
      dir = 1;
    } else {
      dir = -1;
    }

    wep.GetComponent<wepFollowBezier>().resetPos = false;
    wep.transform.parent = null;
    wep.transform.rotation = Quaternion.Euler (0,0,0);
    var tiltAroundZ = Vector3.Angle(wep.transform.position - movePoint.transform.position, movePoint.transform.position - movePoint2.transform.position);
    var target = Quaternion.Euler (0, 0, tiltAroundZ * dir);
    while (wep.transform.position != movePoint.transform.position) {
      wep.transform.rotation = Quaternion.Slerp(wep.transform.rotation, target, (wep.GetComponent<wepFollowBezier>().moveToBaseSpeed / 1) * Time.deltaTime * wep.GetComponent<SwordStats>().spdMult * 1.5f);
      wep.transform.position = Vector2.MoveTowards(wep.transform.position, movePoint.transform.position, wep.GetComponent<wepFollowBezier>().moveToBaseSpeed * Time.deltaTime * wep.GetComponent<SwordStats>().spdMult * 1.5f);
      yield return new WaitForEndOfFrame();
    }
    yield return new WaitForSeconds(.2f);
    wep.GetComponent<wepAttack>().enableAttack = true;
    wep.GetComponent<wepAttack>().damage = dmg;
    while (wep.transform.position != movePoint2.transform.position) {
      wep.transform.position = Vector2.MoveTowards(wep.transform.position, movePoint2.transform.position, wep.GetComponent<wepFollowBezier>().moveToBaseSpeed * 2.25f * Time.deltaTime * wep.GetComponent<SwordStats>().spdMult);
      yield return new WaitForEndOfFrame();
    }
    if (wep.transform.position == movePoint2.transform.position) {
      foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
        if (comp.ability == abilityObj) {
          comp.abilityOn = false;
        }
      }
    }
  }
}
