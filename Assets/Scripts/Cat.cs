using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {
	public float maxSpeed = 1f;
	public float followThreshold = 0.1f;
	public float volleyPower = 0.1f;
	public float difficultyMulti = 0.5f;

	private float dir = -1.0f;
	public static float idleTime = 3.0f;
	private float tempIdle = idleTime;

	public Sprite jump, idle, rest, lose;
	private SpriteRenderer sr;
	private static float spriteChangeTimer = 0.5f;
	private float tempTime = spriteChangeTimer;

	private GameManager gm;

	[SerializeField]
	private GameObject table;
	[SerializeField]
	private AudioClip[] happyClips;
	[SerializeField]
	private AudioClip[] upsetClips;
	[SerializeField]
	private AudioClip winClip;
	[SerializeField]
	private AudioClip loseClip;

	private AudioSource myAudioSource;

	void Awake () {
		gm = Object.FindObjectOfType<GameManager> ();
		myAudioSource = GetComponent<AudioSource>();
		sr = GetComponent<SpriteRenderer> ();

		maxSpeed += (gm.round * difficultyMulti);
	}
	
	// Update is called once per frame
	void Update () {
		tempTime -= Time.deltaTime;
		Move ();
	}

	void Move(){
		if(tempTime < 0){
			SpriteChanger (rest);
		}

		// Only follow the ball if the cat is receiving
		if (!GameManager.ball.IsTowardsCat ()) {
			Idle ();
			return;
		}

		tempIdle = idleTime;

		// Determine how far the cat will go.
		// His target is all the way to the ball (ball.x - cat.x)
		// but limited by maxSpeed in either direction
		float distance = Mathf.Clamp(GameManager.ball.transform.position.x - transform.position.x, -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);

		float targetX = transform.position.x + distance;
		float actualX = Mathf.Clamp (targetX, -table.transform.lossyScale.x * 0.5f, table.transform.lossyScale.x * 0.5f);
		transform.position = new Vector3(actualX, transform.position.y, transform.position.z);
	}

	void Idle(){
		tempIdle -= Time.deltaTime;

		if(tempIdle < 0){
			SpriteChanger (idle);
			if (transform.position.x == table.transform.lossyScale.x * 0.5f || transform.position.x == -table.transform.lossyScale.x * 0.5f) {
				dir *= -1.0f;
				if (transform.position.x == table.transform.lossyScale.x * 0.5f) {
					sr.flipX = false;
				} else {
					sr.flipX = true;
				}
			}

			float distance = Mathf.Clamp (dir * (table.transform.localScale.x * 0.5f), (-maxSpeed / 4) * Time.deltaTime, (maxSpeed / 4) * Time.deltaTime);
			float targetX = transform.position.x + distance;
			float actualX = Mathf.Clamp (targetX, -table.transform.lossyScale.x * 0.5f, table.transform.lossyScale.x * 0.5f);
			transform.position = new Vector3(actualX, transform.position.y, transform.position.z);
		}
	}

	public void PlayHappySound()
	{
		int clipIndex = Random.Range(0, happyClips.Length);
		myAudioSource.PlayOneShot(happyClips[clipIndex]);
	}

	public void PlayUpsetSound()
	{
		int clipIndex = Random.Range(0, upsetClips.Length);
		myAudioSource.PlayOneShot(upsetClips[clipIndex]);
		SpriteChanger (lose);
	}

	public void PlayWinSound()
	{
		myAudioSource.PlayOneShot(winClip);
	}

	public void PlayLoseSound()
	{
		myAudioSource.PlayOneShot(loseClip);
	}

	private void SpriteChanger(Sprite spr){
			sr.sprite = spr;
			tempTime = spriteChangeTimer;
	}

	private void OnCollisionEnter(Collision other){
		if (other.gameObject == GameManager.ball.gameObject) { 
			//ball.Reflect ();
			GameManager.ball.Throw (Random.Range(-0.1f, 0.1f), -volleyPower);
			SpriteChanger (jump);
		}
	}
}
