using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class WepPickup : MonoBehaviour
{

  private void Reset() {
    GetComponent<Collider2D>().isTrigger = true;
    gameObject.layer = 3;
  }

}
