using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public int playerScore { get; private set; }
	public int catScore { get; private set; }
	public float scoreBalance { get; private set; }

	private Cat cat;

	private void Awake()
	{
		cat = FindObjectOfType<Cat>();

		playerScore = 0;
		catScore = 0;
		scoreBalance = 5;
	}

	public void OnPlayerScored()
	{
		playerScore += 1;
		scoreBalance += 1;
		if (scoreBalance >= 10) { cat.PlayLoseSound(); }
		else { cat.PlayUpsetSound(); }
	}

	public void OnCatScored()
	{
		catScore += 1;
		scoreBalance -= 1;
		if (scoreBalance <= 0) { cat.PlayWinSound(); }
		else { cat.PlayHappySound(); }
	}
}
