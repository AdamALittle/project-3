using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class UIHandler : MonoBehaviour
{

  Rigidbody rb = null;
  [SerializeField] GameObject velocityBox = null;
  Text playerVelocity = null;

  private void Awake(){
		rb = GetComponent<Rigidbody>();
    playerVelocity = velocityBox.GetComponent<Text>();
	}

  void Start(){

  }

  void Update(){
    playerVelocity.text = ((int)rb.velocity.magnitude).ToString();
  }
}
