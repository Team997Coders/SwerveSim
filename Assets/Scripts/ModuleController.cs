using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * X is speed and Y is Rotation. Leave Z alone
 */

[RequireComponent(typeof(Rigidbody))]
public class ModuleController : MonoBehaviour {

  public Text angleT;

  public float speed = 0.0f;
  public float angle = 0.0f;

  private Rigidbody rb;

  public void setAngle(float deg) {
    this.angle = deg;
  }

  public void setSpeed(float speed) {
    this.speed = speed;
  }

  public void Start() {
    rb = GetComponent<Rigidbody>();
  }

  public void Update() {
    //angleT.text = angle.ToString();
    transform.localRotation = Quaternion.Euler(0, angle, 90);
    //rb.angularVelocity = new Vector3(speed * 3, 0, 0);
  }

}