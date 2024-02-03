using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wepFollowBezier : MonoBehaviour
{

  public Projectile1Behaviour ProjectilePrefab;

  public GameObject Player;
  public GameObject basePos;
  [SerializeField] private Transform[] routes;
  private int routeToGo;
  private float tParam;
  private Vector2 wepPosition;
  private int isRight;
  [SerializeField] private float speedModifier;
  [SerializeField] private float rotInc;
  [SerializeField] public float moveToBaseSpeed;
  [SerializeField] private float idleAmp;
  [SerializeField] private float idleFreq;
  [SerializeField] private float smooth;
  [SerializeField] private float tiltAngle;
  [SerializeField] private float damage;
  public float projectileSpeed;
  public float projectileDmg;
  [SerializeField] private TrailRenderer tr;
  public bool moveToBase;
  [SerializeField] private bool moveAllowed;
  [SerializeField] public bool resetPos;
  public bool projectileCharged;

  void Start()
  {
    routeToGo = 0;
    tParam = 0f;
    speedModifier = 2.5f;
    moveToBaseSpeed = 12.5f;
    moveAllowed = false;
    resetPos = true;
    moveToBase = false;
    rotInc = -900f;
    idleAmp = 0.001f;
    idleFreq = 3f;
    smooth = 5f;
    tiltAngle = 15f;
    routes = new Transform[1];
    damage = 20;
    projectileCharged = false;
  }

  void Update()
  {
    if (Player == null) {
      basePos = null;
      routes[0] = null;
      return;
    }
    var weapon = Player.GetComponent<Player1Pickup>().pickedWep;
    basePos = Player.GetComponent<Player1Pickup>().WepSlot;
    routes[0] = basePos.transform;
    if (weapon != null) {
      if (weapon.tag == gameObject.tag) {
        if (moveAllowed) {
          StartCoroutine(GoByTheRoute(routeToGo));
        }
        if (moveToBase) {
          ResetWepPos();
        }
        if (resetPos) {
          tiltToPlayer();
        }
      }
    }
  }

  private void tiltToPlayer() {
    if ((Player.GetComponent<Player1Mov>().moving) && (transform.parent != null)) {
      transform.localScale = transform.parent.transform.localScale;
      var tiltAroundZ = Player.GetComponent<Player1Mov>().dirValue * tiltAngle * -1;
      var target = Quaternion.Euler (0, 0, tiltAroundZ);
      transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    } else if ((!Player.GetComponent<Player1Mov>().moving) && (transform.parent != null)) {
      var target = Quaternion.Euler (0, 0, 0);
      transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
      transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * idleFreq) * idleAmp, transform.position.z);
    }
    if (transform.position.x != basePos.transform.position.x) {
      transform.parent = null;
      transform.position = Vector2.MoveTowards(transform.position, basePos.transform.position, moveToBaseSpeed * Time.deltaTime);
      if ((basePos.transform.position.x - transform.position.x) < 0) {
        var tiltAroundZ = tiltAngle * 1;
        var target = Quaternion.Euler (0, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
      } else if ((basePos.transform.position.x - transform.position.x) > 0) {
        var tiltAroundZ = tiltAngle * -1;
        var target = Quaternion.Euler (0, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
      }
    } else if (transform.position == basePos.transform.position) {
      transform.parent = Player.transform;
      transform.localScale = transform.parent.transform.localScale;
    }
  }

  private void ResetWepPos() {
    transform.position = Vector2.MoveTowards(transform.position, basePos.transform.position, moveToBaseSpeed * Time.deltaTime);
    if (basePos.transform.position.x - transform.position.x > 0) {
      transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.Euler(0, 0, -30)), Time.deltaTime * smooth);
    } else if (basePos.transform.position.x - transform.position.x < 0) {
      transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.Euler(0, 0, 30)), Time.deltaTime * smooth);
    }
    if (transform.position == basePos.transform.position) {
      transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.Euler(0, 0, 0)), Time.deltaTime * smooth);
      transform.parent = Player.transform;
      moveToBase = false;
      resetPos = true;
      transform.localScale = transform.parent.transform.localScale;
    }
  }

  private IEnumerator GoByTheRoute(int routeNumber) {
    gameObject.GetComponent<wepAttack>().damage = damage * gameObject.GetComponent<SwordStats>().dmgMult;
    gameObject.GetComponent<wepAttack>().enableAttack = true;
    tr.emitting = true;
    if (Player.GetComponent<Player1Mov>().facingRight) {
      transform.localScale = new Vector3(-1, 1, 1);
      } else {transform.localScale = new Vector3(-1, 1, 1);}
      transform.parent = null;
      transform.eulerAngles = new Vector3(0, 0, 0);
      moveAllowed = false;

      Vector2 p0 = routes[routeNumber].GetChild(0).position;
      Vector2 p1 = routes[routeNumber].GetChild(1).position;
      Vector2 p2 = routes[routeNumber].GetChild(2).position;
      Vector2 p3 = routes[routeNumber].GetChild(3).position;

      while (tParam < 1) {
        if (projectileCharged && tParam > .7f) {
          projectileCharged = false;
          Projectile1Behaviour Projectile = Instantiate(ProjectilePrefab, Player.transform);
          Projectile.GetComponent<Projectile1Behaviour>().speed = projectileSpeed;
          Projectile.GetComponent<Projectile1Behaviour>().dmg = projectileDmg;
          Projectile.GetComponent<Projectile1Behaviour>().dir = isRight;
        }
        transform.Rotate(0, 0, Time.deltaTime * rotInc * isRight);
        tParam += Time.deltaTime * speedModifier;

        wepPosition = Mathf.Pow(1 - tParam, 3) * p0 +
        3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
        3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
        Mathf.Pow(tParam, 3) * p3;

        transform.position = wepPosition;
        yield return new WaitForEndOfFrame();
      }
      tParam = 0f;

      routeToGo += 1;

      if (routeToGo > routes.Length - 1) {
        gameObject.GetComponent<wepAttack>().enableAttack = false;
        tr.emitting = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        moveToBase = true;
      }

    }

    public void FollowTrigger() {
      if (!moveAllowed) {
        if (resetPos) {
          routeToGo = 0;
          resetPos = false;
          moveAllowed = true;
          if (Player.GetComponent<Player1Mov>().facingRight) {
            isRight = 1;
          } else {
            isRight = -1;
          }
        }
      }
    }
  }
