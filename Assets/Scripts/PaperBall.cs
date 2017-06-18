using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBall : MonoBehaviour
{
	private Rigidbody rigidBody;
	private float rotationSpeed = 4f;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		RotateRandomly();
	}

	private void RotateRandomly()
	{
		// Pick a random 3d direction vector of length 1 and multiply it
		// by rotationSpeed, then apply spin using torque
		Vector3 rotationVector = Random.insideUnitSphere * rotationSpeed;
		rigidBody.AddTorque(rotationVector, ForceMode.VelocityChange);
	}
}
