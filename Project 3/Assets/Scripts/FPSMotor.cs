using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour
{

	[SerializeField] Camera _camera = null;
	[SerializeField] float _cameraAngleLimit = 70f;
	[SerializeField] GroundDetector _groundDetector = null;
	bool _isGrounded = false;

	private float _currentCameraRotationX = 0;

	Rigidbody rb = null;

	Vector3 _movementThisFrame = Vector3.zero;
	float _turnAmountThisFrame = 0;
	float _lookAmountThisFrame = 0;

	public event Action Land = delegate { };

	private void Awake(){
		rb = GetComponent<Rigidbody>();
	}

	private void OnEnable(){
		_groundDetector.GroundDetected += OnGroundDetected;
		_groundDetector.GroundVanished += OnGroundVanished;
	}

	private void OnDisable(){
		_groundDetector.GroundDetected -= OnGroundDetected;
		_groundDetector.GroundVanished -= OnGroundVanished;
	}

	public void Move(Vector3 requestedMovement){
		_movementThisFrame = requestedMovement;
	}

	public void SkiMove(){
		rb.AddForce(_camera.transform.forward * 50.0f * Input.GetAxisRaw("Vertical"));
		rb.AddForce(_camera.transform.right * 20.0f * Input.GetAxisRaw("Horizontal"));
	}

	public void Turn(float turnAmount){
		_turnAmountThisFrame = turnAmount;
	}

	public void Look(float lookAmount){
		_lookAmountThisFrame = lookAmount;
	}

	public void Jump(float jumpForce){
		if (_isGrounded == false)
			return;
		rb.AddForce(Vector3.up * jumpForce);
	}

	public void Fly(){
		rb.AddForce(Vector3.up * 125.5f);
	}

	private void FixedUpdate(){
		if (Input.GetKey(KeyCode.Space)){
			SkiMove();
		} else {
			ApplyMovement(_movementThisFrame);
		}
		ApplyTurn(_turnAmountThisFrame);
		ApplyLook(_lookAmountThisFrame);
	}

	void ApplyMovement(Vector3 moveVector){
		if (moveVector == Vector3.zero)
			return;
		rb.MovePosition(rb.position + moveVector);
		_movementThisFrame = Vector3.zero;
	}

	void ApplyTurn(float rotateAmount){
		if (rotateAmount == 0)
			return;

		Quaternion newRotation = Quaternion.Euler(0, rotateAmount, 0);
		rb.MoveRotation(rb.rotation * newRotation);
		_turnAmountThisFrame = 0;
	}

	void ApplyLook(float lookAmount){
		if (lookAmount == 0)
			return;

		_currentCameraRotationX -= lookAmount;
		_currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraAngleLimit, _cameraAngleLimit);
		_camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0, 0);
		_lookAmountThisFrame = 0;
	}

	void OnGroundDetected(){
		_isGrounded = true;
		Land?.Invoke();
	}

	void OnGroundVanished(){
		_isGrounded = false;
	}
}
