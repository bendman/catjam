using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TapHandlers : MonoBehaviour
{
	public delegate void TapHandler(Collider collider, Vector2 position);
	public event TapHandler OnTapDown;

	public delegate void HoldHandler(Collider collider);
	public event HoldHandler OnTapHold;

	public delegate void UpHandler(Vector2 positi2n);
	public event UpHandler OnTapUp;

	public delegate void DragHandler(Vector2 position);
	public event DragHandler OnDrag;

	private static float holdThreshold = 0.1f;
	private float currentTapStartTime = 0;
	private bool isHolding = false;
	private Collider currentTapCollider;

	private void Update()
	{
		HandleDowns();
		HandleHolds();
		HandleUps();
		HandleDrags();
	}

	/// <summary>
	/// Execute for one frame immediately after an initial tap started on the collider.
	/// </summary>
	private void HandleDowns()
	{
		// TODO: maybe this could be static, so it only runs once per frame across instances

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

		// Detect if the player hit the collider
		Ray raycast = Camera.main.ScreenPointToRay(tapPosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(raycast, out raycastHit))
		{
			if (raycastHit.collider.gameObject == gameObject && OnTapDown != null) {
				currentTapStartTime = Time.time;
				currentTapCollider = raycastHit.collider;
				if (OnTapDown != null) { OnTapDown(raycastHit.collider, tapPosition); }
			}
		}
	}

	/// <summary>
	/// Execute for one frame immediately after the collider was tapped and the tap is held for a long press.
	/// </summary>
	private void HandleHolds()
	{
		if (currentTapStartTime != 0 && !isHolding && Time.time - currentTapStartTime > holdThreshold)
		{
			isHolding = true;
			if (OnTapHold != null) { OnTapHold(currentTapCollider); }
		}
	}

	/// <summary>
	/// Execute for one frame immediately after a tap on the collider is released.
	/// </summary>
	private void HandleUps()
	{
		Vector2 tapPosition;

		if (Input.touchSupported && Input.touchCount > 0)
		{
			// Ensure the touch just began
			if (Input.GetTouch(0).phase != TouchPhase.Ended) { return; }
			tapPosition = Input.GetTouch(0).position;
		}
		// Shim for debugging with a mouse
		else if (Input.GetMouseButtonUp(0)) { tapPosition = Input.mousePosition; }
		// Player isn't interacting, so we're done
		else { return; }

		// Reset taps
		currentTapStartTime = 0;
		currentTapCollider = null;
		isHolding = false;

		if (OnTapUp != null) { OnTapUp(tapPosition); }
	}

	private void HandleDrags()
	{
		Vector2 tapPosition;
		if (!isHolding) { return; }

		if (Input.touchSupported && Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Stationary) { return; }
			tapPosition = touch.position;
		}
		else if (Input.GetMouseButton(0)) { tapPosition = Input.mousePosition; }
		else { return; }

		if (OnDrag != null) { OnDrag(tapPosition); }
	}
}
