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

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTapHandlers = GetComponent<TapHandlers>();
		myCollider = GetComponent<SphereCollider>();
	}

	private void OnEnable()
	{
		myTapHandlers.OnTapDown += OnTapDown;
		myTapHandlers.OnTapHold += OnTapHold;
		myTapHandlers.OnTapUp += OnTapUp;
		myTapHandlers.OnDrag += OnDrag;

		Throw(0.1f, 0.1f, -0.1f);
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
		Throw(movement.x, movement.y, movement.y);
	}
	private void OnDrag(Vector2 position)
	{
		Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, dragPosition.z));
		float tableTop = (table.transform.lossyScale.y / 2) + table.transform.position.y;
		targetPosition.y = Mathf.Max(targetPosition.y, table.transform.lossyScale.y);
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
	public void Throw(float x, float y, float z)
	{
		RotateRandomly();
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

	public bool IsTowardsCat()
	{
		return (myRigidbody.velocity.z > 0);
	}
}
