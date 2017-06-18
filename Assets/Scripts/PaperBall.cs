using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBall : MonoBehaviour
{
	private Rigidbody myRigidbody;
	private float rotationSpeed = 4f;

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		RotateRandomly();
		Throw();
	}

	private void Update()
	{
		HandleTaps();
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
	private void Throw()
	{
		myRigidbody.AddForce(0.5f, 0.8f, -1f, ForceMode.Impulse);
	}

	/// <summary>
	/// Reflect the ball back to the source
	/// </summary>
	public void Reflect()
	{
		myRigidbody.velocity = myRigidbody.velocity * -1;
		RotateRandomly();
	}

	/// <summary>
	/// Handle player taps on the ball
	/// </summary>
	private void HandleTaps()
	{
		// TODO: only listen for taps when the ball is near the player
		// TODO: distinguish between taps (reflect) and holds (catch/throw)
		// TODO: tell when the player tapped the ball itself

		Vector2 tapPosition;
		// Get tap position
		if (Input.touchSupported && Input.touchCount > 0)
		{
			// TODO: handle multitouch?
			// Ensure the touch just began
			if (Input.GetTouch(0).phase != TouchPhase.Began) { return; }
			tapPosition = Input.GetTouch(0).position;
		}
		// Shim for debugging with a mouse
		else if (Input.GetMouseButtonDown(0)) { tapPosition = Input.mousePosition; }
		// Player isn't interacting, so we're done
		else { return; }

		// Detect if the player hit the ball
		Ray raycast = Camera.main.ScreenPointToRay(tapPosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(raycast, out raycastHit))
		{
			if (raycastHit.collider.gameObject == gameObject)
			{
				Debug.Log("Hit the ball! " + tapPosition);
				Reflect();
			}
		}

	}
}
