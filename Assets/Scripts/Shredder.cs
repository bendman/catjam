using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
	private PaperBall paperBall;

	private void Awake()
	{
		paperBall = Object.FindObjectOfType<PaperBall>();
	}

	/// <summary>
	/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	private void OnTriggerExit(Collider other)
	{
		// Respawn the ball if it exits the play area
		if (other.gameObject == paperBall.gameObject)
		{
			paperBall.Respawn();
		}
		// Destroy other objects (if any) that leave the play area
		else
		{
			Destroy(other.gameObject);
		}
	}
}
