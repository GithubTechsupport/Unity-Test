using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1Behaviour : MonoBehaviour
{

  public GameObject wep;
  public GameObject parent;

  public float speed;
  public float dmg;

  public int dir;

  void Start() {
    parent = transform.parent.gameObject;
    wep = parent.GetComponent<Player1Pickup>().pickedWep;
    transform.parent = wep.transform;
    transform.localPosition = new Vector3(1,0,0);
    transform.parent = null;
    transform.localScale = new Vector3(-dir, 1, 1);
    gameObject.GetComponent<projectileAttack>().wep = wep;
    gameObject.GetComponent<projectileAttack>().parent = parent;
    gameObject.GetComponent<projectileAttack>().damage = dmg;
    gameObject.GetComponent<projectileAttack>().enableAttack = true;
    Destroy(gameObject, 3);    
  }

    void Update()
    {
      transform.position += transform.right * Time.deltaTime * wep.GetComponent<SwordStats>().spdMult * speed * dir;
    }

    void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject != parent && col.gameObject.name != "Ground" && col.gameObject != wep) {
        Destroy(gameObject);
      }
    }
}
