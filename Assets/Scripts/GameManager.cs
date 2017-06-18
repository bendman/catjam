using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static void LoadStartScene()
	{
		SceneManager.LoadScene(0);
	}

	public static void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public static void LoadEndScene()
	{
		SceneManager.LoadScene(SceneManager.sceneCount - 1);
	}

	private void Awake()
	{
		
	}
	
	private void Update()
	{
		
	}
}
