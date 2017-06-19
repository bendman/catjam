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
		Throw(-0.7f, 0.8f, 1f); // TODO: base throw on drag
	}
	private void OnDrag(Vector2 position)
	{
		// if (!isHolding) { return; }
		Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, dragPosition.z));
		transform.position = targetPosition;
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
