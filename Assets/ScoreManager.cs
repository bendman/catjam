using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public int playerScore { get; private set; }
	public int catScore { get; private set; }
	public float scoreBalance { get; private set; }

	private void Awake()
	{
		playerScore = 0;
		catScore = 0;
		scoreBalance = 5;
	}

	public void OnPlayerScored()
	{
		playerScore += 1;
		scoreBalance += 1;
	}

	public void OnCatScored()
	{
		catScore += 1;
		scoreBalance -= 1;
	}
}
