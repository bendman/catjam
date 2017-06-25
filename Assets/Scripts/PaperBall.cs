using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TapHandlers))]
public class PaperBall : MonoBehaviour
{
	[SerializeField]
	private float rotationSpeed = 4f;
	[SerializeField]
	private GameObject table;
	[SerializeField]
	private AudioClip[] ballCrunchSounds;
	private Vector3 mov;

	private static float reach = 0.5f;
	private static float tableDepth = 2f;
	private Rigidbody myRigidbody;
	private AudioSource myAudioSource;
	private TapHandlers myTapHandlers;
	private bool isHolding = false;
	private Vector3 dragPosition;
	private List<Vector3> previousPositions = new List<Vector3>();
	private SphereCollider myCollider;

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myAudioSource = GetComponent<AudioSource>();
		myTapHandlers = GetComponent<TapHandlers>();
		myCollider = GetComponent<SphereCollider>();

		GameManager.SetBall(this);
	}

	private void OnEnable()
	{
		myTapHandlers.OnTapDown += OnTapDown;
		myTapHandlers.OnTapHold += OnTapHold;
		myTapHandlers.OnTapUp += OnTapUp;
		myTapHandlers.OnDrag += OnDrag;
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

		PlayRandomCrunch();
		previousPositions = new List<Vector3>();
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
		myRigidbody.isKinematic = false;

		if (!IsWithinReach()) { return; }
		Vector3 movement = transform.position - previousPositions[0];
		mov = movement;
		Throw(Mathf.Clamp(movement.x, -0.3f, 0.3f), Mathf.Max(movement.y, 0.01f));
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
			targetPosition.z -= transform.position.y - targetPosition.y;
			targetPosition.y = Mathf.Max(targetPosition.y, tableTop + collisionRadius);
		}

		// Prevent the player from dragging the ball beyond their own reach
		targetPosition.z = Mathf.Min(targetPosition.z, reach * 0.75f);

		transform.position = targetPosition;
		previousPositions.Add(transform.position);
		if (previousPositions.Count > 10) { previousPositions.RemoveAt(0); }
	}

	private bool IsWithinReach()
	{
		// Last check is for when the ball is stuck in the middle
		return isHolding || transform.position.z <= reach || Mathf.Abs(myRigidbody.velocity.z) <= 0.3f;
	}

	private void PlayRandomCrunch()
	{
		int clipIndex = Random.Range(0, ballCrunchSounds.Length);
		myAudioSource.PlayOneShot(ballCrunchSounds[clipIndex]);
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
	public void Throw(float slide, float power)
	{
		bool isTowardsPlayer = transform.position.z > tableDepth / 2;

		// Calculate trajectory to detect if it's going too high
		// float finalHeight = PredictTrajectoryHeight(transform.position, new Vector3(x, y, z), 2f);

		// Reset forces
		myRigidbody.angularVelocity = Vector3.zero;
		myRigidbody.velocity = Vector3.zero;

		// Execute the throw
		RotateRandomly();

		float finalPowerZ = Mathf.Clamp(power, -0.13f, 0.13f);
		float finalSlide = slide;
		float finalPowerUp = Mathf.Min(Mathf.Abs(finalPowerZ), 0.11f);


		// // Ensure the ball is headed out at least as much as it's headed sideways
		// // preventing the player from just throwing it sideways off of the table
		if (Mathf.Abs(finalSlide) > finalPowerUp)
		{
			finalSlide = finalSlide < 0 ? -finalPowerUp : finalPowerUp;
		}

		// Ensure the ball doesn't go sideways enough to go off of the table
		// Do this by clamping the ball more away from the edge it starts nearest to
		// Examples:
		// Ball position -2 means (0 / 2) * -0.3 < finalSlide < (4 / 2) * 0.3
		// Ball position 2 means finalSlide < 0
		// Ball position -1.5 means finalSlide > (1.5 / 2) * 0.3
		float slideRange = isTowardsPlayer ? 2.4f : 4f;
		float initialX = transform.position.x;
		float distFromLeft = -slideRange / 2 - initialX;
		float distFromRight = slideRange / 2 - initialX;
		float leftMaxAngle = Mathf.Atan2(distFromLeft, tableDepth);
		float rightMaxAngle = Mathf.Atan2(distFromRight, tableDepth);
		float leftMaxSlide = Mathf.Tan(leftMaxAngle) * Mathf.Abs(finalPowerZ);
		float rightMaxSlide = Mathf.Tan(rightMaxAngle) * Mathf.Abs(finalPowerZ);

		finalSlide = Mathf.Clamp(finalSlide, leftMaxSlide, rightMaxSlide);

		myRigidbody.AddForce(finalSlide, finalPowerUp, finalPowerZ, ForceMode.Impulse);
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
	// public void Reflect()
	// {
	// 	myRigidbody.velocity = myRigidbody.velocity * -1;
	// 	RotateRandomly();
	// }

	public bool IsTowardsCat()
	{
		return (myRigidbody.velocity.z > 0);
	}
}
