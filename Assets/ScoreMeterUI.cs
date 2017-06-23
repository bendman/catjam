using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMeterUI : MonoBehaviour
{
	private ScoreManager scoreManager;
	private Slider mySlider;

	private void Awake()
	{
		scoreManager = FindObjectOfType<ScoreManager>();
		mySlider = GetComponent<Slider>();
	}

	private void Update()
	{
		mySlider.value = scoreManager.scoreBalance;
	}
}
