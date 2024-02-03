using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterimageSprite : MonoBehaviour
{

  private float activeTime;
  private float timeActivated;
  private float alpha;
  private float alphaSet;
  private float alphaMultiplier;

  [SerializeField] public Transform player;

  private SpriteRenderer SR;
  private SpriteRenderer playerSR;

  private Color color;

  private void OnEnable() {
    activeTime = 0.1f;
    alphaSet = 0.8f;
    alphaMultiplier = 0.98f;
    player = GameObject.Find("Player1").transform;
    SR = GetComponent<SpriteRenderer>();
    playerSR = player.GetComponent<SpriteRenderer>();

    alpha = alphaSet;
    SR.sprite = playerSR.sprite;
    transform.position = player.position;
    transform.localScale = player.localScale;
    timeActivated = Time.time;

  }

  private void Update() {
    alpha *= alphaMultiplier;
    color = new Color(1f,1f,1f,alpha);
    SR.color = color;

    if (Time.time >= (timeActivated + activeTime)) {
      PlayerAfterimagePool.Instance.AddToPool(gameObject);
    }
  }
}
