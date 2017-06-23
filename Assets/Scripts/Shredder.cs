using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
	private BallSpawn ballSpawn;

	private void Awake()
	{
		ballSpawn = Object.FindObjectOfType<BallSpawn>();
	}

	/// <summary>
	/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	private void OnTriggerExit(Collider other)
	{
		// Respawn the ball if it exits the play area
		if (other.gameObject == GameManager.ball.gameObject)
		{
			Destroy(GameManager.ball.gameObject);
			ballSpawn.SpawnBall();
		}
		// Destroy other objects (if any) that leave the play area
		else
		{
			Destroy(other.gameObject);
		}
	}
}
