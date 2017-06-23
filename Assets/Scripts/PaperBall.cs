using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TapHandlers))]
public class PaperBall : MonoBehaviour
{
	[SerializeField]
	private float rotationSpeed = 4f;
	[SerializeField]
	private GameObject table;

	private Rigidbody myRigidbody;
	private TapHandlers myTapHandlers;
	private bool isHolding = false;
	private Vector3 dragPosition;
	private List<Vector3> previousPositions = new List<Vector3>();
	private SphereCollider myCollider;
	private BallSpawn ballSpawn;

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTapHandlers = GetComponent<TapHandlers>();
		myCollider = GetComponent<SphereCollider>();
		ballSpawn = Object.FindObjectOfType<BallSpawn>();
	}

	private void OnEnable()
	{
		myTapHandlers.OnTapDown += OnTapDown;
		myTapHandlers.OnTapHold += OnTapHold;
		myTapHandlers.OnTapUp += OnTapUp;
		myTapHandlers.OnDrag += OnDrag;

		Respawn();
	}

	private void OnDisable()
	{
		myTapHandlers.OnTapDown -= OnTapDown;
		myTapHandlers.OnTapHold -= OnTapHold;
		myTapHandlers.OnTapUp -= OnTapUp;
		myTapHandlers.OnDrag -= OnDrag;
	}

	private void OnTapDown(Collider collider, Vector2 position) {
		if (!IsWithinReach()) { return; }
		Reflect();
	}
	private void OnTapHold(Collider collider)
	{
		if (!IsWithinReach()) { return; }
		isHolding = true;
		myRigidbody.isKinematic = true;
		myRigidbody.velocity = Vector3.zero;
		dragPosition = Camera.main.WorldToScreenPoint(transform.position);
	}
	private void OnTapUp(Vector2 position)
	{
		if (!isHolding) { return; }
		isHolding = false;

		if (!IsWithinReach()) { return; }
		myRigidbody.isKinematic = false;
		Vector3 movement = transform.position - previousPositions[0];
		Throw(movement.x, movement.y, movement.y);
	}
	private void OnDrag(Vector2 position)
	{
		if (!IsWithinReach()) { return; }
		Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, dragPosition.z));

		// Handle collisions with the table top
		float tableTop = (table.transform.lossyScale.y / 2) + table.transform.position.y;
		float collisionRadius = myCollider.radius * transform.localScale.y;
		if (targetPosition.y <= tableTop + collisionRadius)
		{
			targetPosition.z += targetPosition.y - transform.position.y;
			targetPosition.y = Mathf.Max(targetPosition.y, tableTop + collisionRadius);
		}

		transform.position = targetPosition;
		previousPositions.Add(transform.position);
		if (previousPositions.Count > 10) { previousPositions.RemoveAt(0); }
	}

	private bool IsWithinReach()
	{
		return isHolding || transform.position.z <= 0.5f;
	}

	/// <summary>
	/// Set the ball rotating at a random angle
	/// </summary>
	private void RotateRandomly()
	{
		Vector3 rotationVector = Random.insideUnitSphere * rotationSpeed;
		myRigidbody.AddTorque(rotationVector, ForceMode.VelocityChange);
	}

	/// <summary>
	/// Throw the ball
	/// </summary>
	public void Throw(float x, float y, float z)
	{
		// Calculate trajectory to detect if it's going too high
		// float finalHeight = PredictTrajectoryHeight(transform.position, new Vector3(x, y, z), 2f);

		// Reset forces
		myRigidbody.angularVelocity = Vector3.zero;
		myRigidbody.velocity = Vector3.zero;

		// Execute the throw
		RotateRandomly();
		myRigidbody.AddForce(
			x,
			Mathf.Min(y, 0.13f),
			Mathf.Min(z, 0.13f),
			ForceMode.Impulse
		);
	}

	// public static float PredictTrajectoryHeight(Vector3 initialPosition, Vector3 forceVector, float measurePointZ)
	// {
	// 	float zDistance = measurePointZ - initialPosition.z;
	// 	float gravity = Physics.gravity.y * -1;
	// 	float height = 0f;

	// 	// Calculate throw angle given input forces
	// 	// http://www.mathopenref.com/arctan.html
	// 	float throwAngle = Mathf.Atan(forceVector.y / forceVector.z);

	// 	// Calculate the velocity (ignore the x component, as it doesn't matter for cross-table velocity)
	// 	// https://en.wikipedia.org/wiki/Pythagorean_theorem
	// 	float velocity = Mathf.Sqrt(Mathf.Pow(forceVector.z, 2) + Mathf.Pow(forceVector.y, 2)) * 10;

	// 	// Formula to calculate trajectory
	// 	// https://en.wikipedia.org/wiki/Trajectory_of_a_projectile (note: their x is our z because 2d vs 3d)
	// 	height = initialPosition.y + (zDistance * Mathf.Tan(throwAngle)) - ((gravity * Mathf.Pow(zDistance, 2)) / (2 * Mathf.Pow(velocity * Mathf.Cos(throwAngle), 2)));

	// 	return height;
	// }

	/// <summary>
	/// Reflect the ball back to the source
	/// </summary>
	private void Reflect()
	{
		myRigidbody.velocity = myRigidbody.velocity * -1;
		RotateRandomly();
	}

	public bool IsTowardsCat()
	{
		return (myRigidbody.velocity.z > 0);
	}

	public void Respawn()
	{
		// Reset state
		isHolding = false;

		// Stop its movement
		myRigidbody.velocity = Vector3.zero;
		myRigidbody.angularVelocity = Vector3.zero;

		// Place it on the table in front of the player
		transform.position = ballSpawn.transform.position;
	}
}
