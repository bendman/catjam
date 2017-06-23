using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScore : MonoBehaviour
{
	private ScoreManager scoreManager;

	private void Awake()
	{
		scoreManager = FindObjectOfType<ScoreManager>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject == GameManager.ball.gameObject)
		{
			scoreManager.OnCatScored();
		}
	}
}
