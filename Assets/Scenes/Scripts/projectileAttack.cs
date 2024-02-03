using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileAttack : MonoBehaviour
{

  [SerializeField] public bool enableAttack;
  [SerializeField] public bool attacking;
  [SerializeField] public float damage;
  [SerializeField] public float cd;
  public GameObject wep;
  public GameObject parent;

  void Awake() {
    enableAttack = false;
    attacking = false;
    damage = 0;
    cd = .1f;
  }

  void Update()
  {

  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (enableAttack && !attacking) {
      if (col.gameObject.layer == 7 && col.gameObject.name != parent.name) {
        damage *= wep.GetComponent<SwordStats>().dmgMult;
        Debug.Log("Damaged " + col.gameObject.name + " for " + damage + " damage");
        col.gameObject.GetComponent<PlayerStats>().damageSource = parent;
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
