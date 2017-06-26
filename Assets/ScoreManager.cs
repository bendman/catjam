using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public int playerScore { get; private set; }
	public int catScore { get; private set; }
	public float scoreBalance { get; private set; }

	public GameManager gm;

	private Cat cat;

	private void Awake()
	{
		gm = Object.FindObjectOfType<GameManager> ();
		cat = FindObjectOfType<Cat>();

		playerScore = 0;
		catScore = 0;
		scoreBalance = 5;
	}

	void Update()
	{
		if (scoreBalance >= 10) {
			gm.ReloadScene ();
			++gm.round;
		}
		if (gm.round == gm.numOfRounds - 1) {
			if (playerScore > catScore) {

			} else {

			}
			gm.LoadEndScene ();
			gm.round = 0;
		}
	}

	public void OnPlayerScored()
	{
		playerScore += 1;
		scoreBalance += 1;
		if (scoreBalance >= 10) { 
			cat.PlayLoseSound();

		}
		else { cat.PlayUpsetSound(); }
	}

	public void OnCatScored()
	{
		catScore += 1;
		scoreBalance -= 1;
		if (scoreBalance <= 0) {
			cat.PlayWinSound(); 
		}
		else { cat.PlayHappySound(); }
	}
}
