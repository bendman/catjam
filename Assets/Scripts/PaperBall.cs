using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TapHandlers))]
public class PaperBall : MonoBehaviour
{
	[SerializeField]
	private float rotationSpeed = 4f;

	private Rigidbody myRigidbody;
	private TapHandlers myTapHandlers;
	private bool isHolding = false;
	private Vector3 dragPosition;
	private List<Vector3> previousPositions = new List<Vector3>();

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTapHandlers = GetComponent<TapHandlers>();
	}

	private void OnEnable()
	{
		RotateRandomly();
		Throw(0.5f, 0.8f, -1f);
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

	private void OnTapDown(Collider collider, Vector2 position) { Reflect(); }
	private void OnTapHold(Collider collider)
	{
		isHolding = true;
		myRigidbody.isKinematic = true;
		myRigidbody.velocity = Vector3.zero;
		dragPosition = Camera.main.WorldToScreenPoint(transform.position);
	}
	private void OnTapUp(Vector2 position)
	{
		if (!isHolding) { return; }
		myRigidbody.isKinematic = false;
		Vector3 movement = transform.position - previousPositions[0];
		Debug.Log(movement);
		Throw(movement.x * 5, movement.y * 5, movement.y * 5);
	}
	private void OnDrag(Vector2 position)
	{
		Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, dragPosition.z));
		transform.position = targetPosition;
		previousPositions.Add(transform.position);
		if (previousPositions.Count > 10) { previousPositions.RemoveAt(0); }
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
	/// Throw the ball towards the player
	/// </summary>
	private void Throw(float x, float y, float z)
	{
		myRigidbody.AddForce(x, y, z, ForceMode.Impulse);
	}

	/// <summary>
	/// Reflect the ball back to the source
	/// </summary>
	private void Reflect()
	{
		myRigidbody.velocity = myRigidbody.velocity * -1;
		RotateRandomly();
	}
}
