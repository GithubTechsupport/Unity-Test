using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

  [SerializeField] public float maxHp;
  [SerializeField] public float currentHp;
  [SerializeField] private float knockbackSpeedX;
  [SerializeField] private float knockbackSpeedY;
  [SerializeField] private float knockbackDuration;
  private float knockbackStart;

  public bool knockback;
  public bool dead;

  Vector3 respawnPosition;
  Quaternion respawnRotation;

  public GameObject damageSource;
  public GameObject hpbar;

  private Rigidbody2D rb;

  void Start()
  {
    maxHp = 100;
    currentHp = maxHp;
    knockback = false;
    rb = GetComponent<Rigidbody2D>();
    knockbackSpeedX = 10;
    knockbackSpeedY = 10;
    knockbackDuration = 0.2f;
    dead = false;
    respawnPosition = transform.position;
    respawnRotation = transform.rotation;
    hpbar = transform.Find("healthBar").gameObject;
    hpbar.SetActive(false);
  }

  public void takeDmg(float dmg) {
    getKnockback();
    if (dead) {return;}

    currentHp -= dmg;
    hpbar.SetActive(true);

    if (currentHp <= 0) {
      die();
    }
    damageSource = null;
  }

  void getKnockback() {
    knockback = true;
    knockbackStart = Time.time;

    if (damageSource.transform.position.x - transform.position.x < 0) {
      Debug.Log(gameObject.name + " was hit from left!");
      rb.velocity = new Vector2(knockbackSpeedX, knockbackSpeedY);
    } else {
      Debug.Log(gameObject.name + " was hit from right!");
      rb.velocity = new Vector2(-knockbackSpeedX, knockbackSpeedY);
    }
  }

  void checkKnockback() {
    if (Time.time >= knockbackStart + knockbackDuration && knockback) {
      knockback = false;
      rb.velocity = new Vector2(0.0f, rb.velocity.y);
    }
  }

  void die() {
    Debug.Log(gameObject.name + " has died!");
    if (rb.velocity == new Vector2(knockbackSpeedX, knockbackSpeedY)) {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler (0, 0, 90), Time.deltaTime * 100);
      } else {transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler (0, 0, -90), Time.deltaTime * 100);}
      StartCoroutine(respawn());
    }

    private IEnumerator respawn() {
      dead = true;
      yield return new WaitForSeconds(3);
      dead = false;
      transform.position = respawnPosition;
      transform.rotation = respawnRotation;
      currentHp = maxHp;
    }

    void updateHealthbar() {
      hpbar.transform.Find("bar").gameObject.transform.localScale = new Vector3(currentHp / maxHp, 1f);
      if (dead) {
        hpbar.SetActive(false);
      }
    }

    void Update()
    {
      checkKnockback();
      if (hpbar.activeInHierarchy) {
        updateHealthbar();
      }
    }
  }
