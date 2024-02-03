using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player1Pickup : MonoBehaviour
{

  PhotonView view;

  public GameObject detectedWep;
  public GameObject pickedWep;
  public GameObject WepSlot;
  public Transform Wep1Slot;
  public Transform detectionPointA;
  public Transform detectionPointB;
  private const float detectionRadius = 10f;
  public LayerMask detectionLayer;
  public bool picked;
  private float pickedWepYValue;

  public KeyCode pickupKey;
  public KeyCode attackKey;

  void Start() {
    view = GetComponent<PhotonView>();
  }

  public void Pickup() {
    if(picked) {
      picked = false;
      pickedWep.GetComponent<wepFollowBezier>().Player = null;
      pickedWep.transform.parent = null;
      pickedWep.transform.position = new Vector3(pickedWep.transform.position.x, pickedWepYValue, pickedWep.transform.position.z);
      pickedWep.transform.eulerAngles = new Vector3(0, 0, 0);
      pickedWep = null;

    } else {
      picked = true;
      pickedWep = detectedWep;
      pickedWep.GetComponent<wepFollowBezier>().Player = gameObject;
      pickedWep.transform.parent = transform;
      pickedWepYValue = pickedWep.transform.position.y;
      pickedWep.transform.localPosition = Wep1Slot.transform.localPosition;
      if (transform.GetComponent<Player1Mov>().facingRight) {
        pickedWep.transform.localScale = new Vector3(-1, 1, 1);
      } else {pickedWep.transform.localScale = new Vector3(-1, 1, 1);}

    }
  }

  bool detectPickupInput() {
    return Input.GetKeyDown(pickupKey);
  }

  bool detectAttackInput() {
    return Input.GetKeyDown(attackKey);
  }

  bool detectWep() {

    Collider2D obj = Physics2D.OverlapArea(detectionPointA.position,detectionPointB.position,detectionLayer);
    if (obj==null) {
      detectedWep = null;
      return false;
    } else {
      detectedWep = obj.gameObject;
      return true;
    }
  }



  void Update()
  {
    if (view.IsMine) {
    if (gameObject.GetComponent<PlayerStats>().dead) {
      return;
    }
    if (detectWep()) {
      if (detectPickupInput()) {
        if ((pickedWep == null) || (pickedWep.GetComponent<wepFollowBezier>().resetPos)) {
          Pickup();
        }
      }
    } else if (!detectWep()) {
      if (detectPickupInput() && (pickedWep != null) && (pickedWep.GetComponent<wepFollowBezier>().resetPos)) {
        Pickup();
      }
    }
    if (pickedWep != null && detectAttackInput()) {
      pickedWep.GetComponent<wepFollowBezier>().FollowTrigger();
    }
  }
  }
}
