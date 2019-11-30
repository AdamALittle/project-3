﻿using UnityEngine;
using System;

public class FPSInput : MonoBehaviour
{
	public event Action<Vector3> MoveInput = delegate { };
	public event Action<Vector3> RotateInput = delegate { };
	public event Action JumpInput = delegate { };

	void Update(){
		DetectMoveInput();
		DetectRotateInput();
		DetectJumpInput();
		DetectRMB();
		DetectEscape();
	}

	void DetectMoveInput(){
		// process input as a 0 or 1 value.
		float xInput = Input.GetAxisRaw("Horizontal");
		float yInput = Input.GetAxisRaw("Vertical");

		// if we have either horizontal or vertical input
		if (xInput != 0 || yInput != 0){
			Vector3 _horizontalMovement = transform.right * xInput;
			Vector3 _forwardMovement = transform.forward * yInput;

			// need to convert multiple movements into a single vector.
			Vector3 movement = (_horizontalMovement + _forwardMovement).normalized;
			MoveInput?.Invoke(movement);
		}
	}

	void DetectRotateInput(){
		float xInput = Input.GetAxisRaw("Mouse X");
		float yInput = Input.GetAxisRaw("Mouse Y");

		if (xInput != 0 || yInput != 0){
			Vector3 rotation = new Vector3(yInput, xInput, 0);
			RotateInput?.Invoke(rotation);
		}
	}

	void DetectJumpInput(){
		if (Input.GetKeyDown(KeyCode.Space)){
			JumpInput?.Invoke();
		}
	}

	void DetectRMB(){
		if (Input.GetMouseButton(1)){
			//do nothing
		}
	}

	void DetectEscape(){
		if (Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
	}

}
