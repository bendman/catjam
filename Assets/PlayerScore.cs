using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
	private ScoreManager scoreManager;
	private PaperBall paperBall;

	private void Awake()
	{
		scoreManager = FindObjectOfType<ScoreManager>();
		paperBall = FindObjectOfType<PaperBall>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject == paperBall.gameObject)
		{
			scoreManager.OnPlayerScored();
		}
	}
}
