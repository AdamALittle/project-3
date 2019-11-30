using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{
	public event Action GroundDetected = delegate { };
	public event Action GroundVanished = delegate { };
	
	private void OnTriggerEnter(Collider col){
		GroundDetected?.Invoke();
	}
	
	private void OnTriggerExit(Collider col){
		GroundVanished?.Invoke();
	}
}
