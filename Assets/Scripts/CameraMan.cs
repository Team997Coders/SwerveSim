using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour {

  public Transform follow;

  private Vector3 offset;

  public void Start() {
    offset = transform.position - follow.position;
  }

  public void FixedUpdate() {
    transform.position = offset + follow.position;
  }

  public void Yeet() {
    Application.Quit();
  }

}