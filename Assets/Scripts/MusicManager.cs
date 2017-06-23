using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip launchMusic;
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
		PlayLaunchMusic();
	}

	private void PlayLaunchMusic()
	{
		myAudioSource.clip = launchMusic;
		myAudioSource.loop = false;
		myAudioSource.PlayOneShot(myAudioSource.clip, 2f);
		Invoke("PlayOnce", myAudioSource.clip.length / 2);
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