using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FPSMotor))]
public class UIHandler : MonoBehaviour
{

  Rigidbody rb = null;
  FPSMotor pc = null;
  [SerializeField] GameObject velocityBox = null;
  [SerializeField] GameObject fuelBox = null;
  Text playerVelocity = null;
  Text playerFuel = null;

  private void Awake(){
		rb = GetComponent<Rigidbody>();
    pc = GetComponent<FPSMotor>();
    playerVelocity = velocityBox.GetComponent<Text>();
    playerFuel = fuelBox.GetComponent<Text>();
	}

  void Start(){

  }

  void Update(){
    playerVelocity.text = ((int)rb.velocity.magnitude).ToString();
    playerFuel.text = ((int)pc.jetFuel).ToString();
  }
}
