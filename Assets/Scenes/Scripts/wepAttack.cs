using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wepAttack : MonoBehaviour
{

  [SerializeField] public bool enableAttack;
  [SerializeField] public bool attacking;
  [SerializeField] public float damage;
  [SerializeField] public float cd;

  void Awake() {
    enableAttack = false;
    attacking = false;
    damage = 0;
    cd = .1f;
  }

  void Update()
  {

  }

  private void OnTriggerStay2D(Collider2D col) {
    if (enableAttack && !attacking) {
      if (col.gameObject.layer == 7 && col.gameObject.name != gameObject.GetComponent<wepFollowBezier>().Player.name) {
        Debug.Log("Damaged " + col.gameObject.name + " for " + damage + " damage");
        col.gameObject.GetComponent<PlayerStats>().damageSource = gameObject.GetComponent<wepFollowBezier>().Player;
        col.gameObject.GetComponent<PlayerStats>().takeDmg(damage);
        StartCoroutine(damageCooldown(cd));
      }
    }
  }

  private IEnumerator damageCooldown(float cdValue) {
    attacking = true;
    yield return new WaitForSeconds(cdValue);
    attacking = false;
  }
}
