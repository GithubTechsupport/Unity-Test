using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffectScript : MonoBehaviour
{

  private SpriteRenderer SR;

  float alpha;

  Color color;

  public float minX;
  public float maxX;
  public float minY;
  public float maxY;
  public float dmg;

  public GameObject wep;
  public GameObject parent;

  void Start()
  {
    transform.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    alpha = 1;
    SR = gameObject.GetComponent<SpriteRenderer>();
    transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
    gameObject.GetComponent<projectileAttack>().wep = wep;
    gameObject.GetComponent<projectileAttack>().parent = parent;
    gameObject.GetComponent<projectileAttack>().damage = dmg;
    gameObject.GetComponent<projectileAttack>().enableAttack = true;
  }

  private void Update() {
    alpha *= 0.95f;
    color = new Color(1f,1f,1f,alpha);
    SR.color = color;
    transform.position += transform.up * Time.deltaTime;
    if (alpha <= .1) {
      Destroy(gameObject);
    }
  }
}
