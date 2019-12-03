using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour
{

	[SerializeField] Camera _camera = null;
	[SerializeField] float _cameraAngleLimit = 70f;
	[SerializeField] GroundDetector _groundDetector = null;
	[SerializeField] GameObject velocityBox = null;
	[SerializeField] GameObject velocityBack = null;
	bool _isGrounded = false;

	private float _currentCameraRotationX = 0;

	Rigidbody rb = null;

	Vector3 _movementThisFrame = Vector3.zero;
	float _turnAmountThisFrame = 0;
	float _lookAmountThisFrame = 0;

	public event Action Land = delegate { };

	public float jetFuel = 100f;

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
		if (_isGrounded == false){
			if (Vector3.Dot(rb.velocity, _camera.transform.forward) <= 15){
				rb.AddForce(_camera.transform.forward * 50.0f * Input.GetAxisRaw("Vertical"));
			}
		}
		rb.AddForce(_camera.transform.right * 50.0f * Input.GetAxisRaw("Horizontal"));
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
		if (jetFuel > 1){
			rb.AddForce(Vector3.up * 200f);
			jetFuel -= 1.25f;
		}
	}

	private void FixedUpdate(){
		if (jetFuel < 100){
			jetFuel += .5f;
		}
		if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.magnitude <= 30f){
			// When pressing space, recalculate velocity based on non-force movement
			rb.velocity = _movementThisFrame * 40f + rb.velocity;
		}
		if (Input.GetKey(KeyCode.Space)){
			velocityBack.SetActive(true);
			velocityBox.SetActive(true);
			SkiMove();
		} else {
			velocityBox.SetActive(false);
			velocityBack.SetActive(false);
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
