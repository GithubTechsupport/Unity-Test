using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingPersonnelKidMono : MonoBehaviour
{

  public SlashEffectScript SlashPrefab;
  public GameObject[] Slashes;

  public GameObject wep;
  public GameObject movePoint;
  public GameObject movePoint2;
  public GameObject boxPoint;
  public GameObject boxPoint2;
  public GameObject parent;

  private Rigidbody2D rb;

  public Object abilityObj;

  Vector3 origPos;

  public int dir;

  public float dmg;
  public float hbxh;
  public int dist;
  public float origVel;
  public float newVel;
  float originalGravity;
  float lastXpos;

  public bool startAbility;


  void Start()
  {
    startAbility = false;
    Slashes = new GameObject[100];
  }

  void Update()
  {
    if (startAbility) {
      StartCoroutine(ability());
    }
  }

  private IEnumerator ability() {
    rb = parent.GetComponent<Rigidbody2D>();
    origVel = parent.GetComponent<Player1Mov>().velocityP1;
    parent.GetComponent<Player1Mov>().velocityP1 = newVel;
    originalGravity = rb.gravityScale;
    startAbility = false;
    movePoint = new GameObject("movePoint");
    movePoint2 = new GameObject("movePoint2");
    movePoint.transform.parent = parent.transform;
    movePoint2.transform.parent = parent.transform;
    movePoint.transform.localPosition = new Vector3(1f,0f,0);
    movePoint2.transform.localPosition = new Vector3(dist,0,0);
    wep = parent.GetComponent<Player1Pickup>().pickedWep;
    wep.GetComponent<wepFollowBezier>().resetPos = false;
    wep.transform.parent = parent.transform;
    wep.transform.rotation = Quaternion.Euler (0,0,0);
    if (parent.GetComponent<Player1Mov>().facingRight) {
      dir = -1;
    } else {
      dir = 1;
    }
    var target = Quaternion.Euler (0, 0, 210 * dir);
    parent.GetComponent<Player1Mov>().stopRot = true;
    while (wep.transform.position != movePoint.transform.position) {
      if (parent.GetComponent<Player1Mov>().facingRight && parent.GetComponent<Player1Mov>().dirValue < 0)
      {
        wep.transform.localScale = new Vector3(wep.transform.localScale.x * -1, wep.transform.localScale.y, wep.transform.localScale.z);
      }
      else if (!parent.GetComponent<Player1Mov>().facingRight && parent.GetComponent<Player1Mov>().dirValue > 0)
      {
        wep.transform.localScale = new Vector3(wep.transform.localScale.x * -1, wep.transform.localScale.y, wep.transform.localScale.z);
      }
      wep.transform.rotation = Quaternion.Slerp(wep.transform.rotation, target, (wep.GetComponent<wepFollowBezier>().moveToBaseSpeed / 1) * Time.deltaTime * 0.7f);
      wep.transform.position = Vector2.MoveTowards(wep.transform.position, movePoint.transform.position, wep.GetComponent<wepFollowBezier>().moveToBaseSpeed * Time.deltaTime * 0.7f);
      yield return new WaitForEndOfFrame();
    }
    if (wep.transform.position == movePoint.transform.position) {
      if (parent.GetComponent<Player1Mov>().facingRight) {
        dir = -1;
      } else {
        dir = 1;
      }
      wep.transform.rotation = Quaternion.Euler (0, 0, 210 * dir);
    }
    yield return new WaitForSeconds(1.5f);
    boxPoint = new GameObject("boxPoint");
    boxPoint2 = new GameObject("boxPoint2");
    boxPoint.transform.parent = parent.transform;
    boxPoint2.transform.parent = parent.transform;
    boxPoint.transform.localPosition = new Vector3(movePoint.transform.localPosition.x,movePoint.transform.localPosition.y,0);
    boxPoint2.transform.localPosition = new Vector3(movePoint2.transform.localPosition.x - 2,movePoint2.transform.localPosition.y + hbxh,0);
    boxPoint.transform.parent = null;
    boxPoint2.transform.parent = null;
    movePoint.transform.parent = null;
    movePoint2.transform.parent = null;
    parent.GetComponent<Player1Mov>().dashing = true;
    rb.gravityScale = 0f;
    float pointDist = Vector3.Distance(parent.transform.position, movePoint2.transform.position);
    origPos = new Vector3(0,0,0);
    lastXpos = parent.transform.position.x;
    PlayerAfterimagePool.Instance.GetFromPool();
    while (pointDist > 2 && origPos != parent.transform.position || pointDist > 2 && rb.velocity != new Vector2(0,0)) {
      origPos = parent.transform.position;
      pointDist = Vector3.Distance(parent.transform.position, movePoint2.transform.position);
      rb.velocity = new Vector2(parent.transform.localScale.x * 200f, 0f);
      if (Mathf.Abs(transform.position.x - lastXpos) > .25) {
        PlayerAfterimagePool.Instance.GetFromPool();
        lastXpos = transform.position.x;
      }
      yield return new WaitForEndOfFrame();
    }
    if (pointDist <= 2 || origPos == parent.transform.position || rb.velocity == new Vector2(0,0)) {
      rb.velocity = new Vector2(0f, 0f);
      for(int i = 0; i <= dist * 2; i++)
      {
        SlashEffectScript Slash = Instantiate(SlashPrefab);
        Slashes[i] = Slash.gameObject;
        Slashes[i].GetComponent<SlashEffectScript>().parent = parent;
        Slashes[i].GetComponent<SlashEffectScript>().wep = wep;
        Slashes[i].GetComponent<SlashEffectScript>().dmg = dmg;
      }
      foreach(GameObject obj in Slashes) {
        if (obj == null) {
          break;
        }
        obj.GetComponent<SlashEffectScript>().minX = boxPoint.transform.position.x;
        obj.GetComponent<SlashEffectScript>().maxX = boxPoint2.transform.position.x;
        obj.GetComponent<SlashEffectScript>().minY = boxPoint.transform.position.y;
        obj.GetComponent<SlashEffectScript>().maxY = boxPoint2.transform.position.y;
      }
    }
    yield return new WaitForSeconds(1f);
    rb.gravityScale = originalGravity;
    parent.GetComponent<Player1Mov>().velocityP1 = origVel;
    foreach (AbilityHolder comp in parent.GetComponents<AbilityHolder>()) {
      if (comp.ability == abilityObj) {
        comp.abilityOn = false;
      }
    }
    wep.GetComponent<wepFollowBezier>().resetPos = true;
    parent.GetComponent<Player1Mov>().dashing = false;
    parent.GetComponent<Player1Mov>().stopRot = false;
  }

}
