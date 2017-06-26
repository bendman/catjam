using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Singleton GameManager
	protected static GameManager instance = null;

	public static PaperBall ball { get; private set; }

	public int round = 0;
	public int numOfRounds = 2;

	private void Awake()
	{

		// Enforce singleton pattern
		if (instance == null) { instance = this; }
		else {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(instance);
	}

	public static void SetBall(PaperBall newBall)
	{
		ball = newBall;
	}

	// Public instance methods, since UI can't access statics
	// http://answers.unity3d.com/questions/840906/ugui-ui-button-onclick-static-methods.html
	public void LoadStartScene()
	{
		Debug.Log("Loading Start Scene");
		SceneManager.LoadScene(0);
	}

	public void ReloadScene()
	{
		Debug.Log ("Reloading Scene");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadNextLevel()
	{
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
		Debug.Log("Loading Next Scene: " + nextSceneIndex.ToString());
		SceneManager.LoadScene(nextSceneIndex);
	}

	public void LoadEndScene()
	{
		Debug.Log("Loading End Scene");
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
	}
}