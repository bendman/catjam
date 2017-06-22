using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip[] startMusic;
	[SerializeField]
	private AudioClip[] gameplayMusic;

	private AudioSource myAudioSource;

	protected static MusicManager instance = null;

	private void Awake()
	{
		if (instance == null) { instance = this; }
		else {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(instance);

		myAudioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		PlayOnce();
	}

	public void PlayOnce()
	{
		myAudioSource.clip = startMusic[0];
		myAudioSource.loop = false;
		myAudioSource.Play();
		Invoke("PlayLoop", myAudioSource.clip.length);
	}

	private void PlayLoop()
	{
		myAudioSource.clip = startMusic[1];
		myAudioSource.loop = true;
		myAudioSource.Play();
	}

}