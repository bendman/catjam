using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {
	public float maxSpeed = 2.0f;
	public float followThreshold = 0.1f;

	[SerializeField]
	private GameObject table;
	[SerializeField]
	private PaperBall ball;

	void Awake () {
		//table = GameObject.Find ("Table");
		ball = FindObjectOfType<PaperBall>();
		//ball = GameObject.Find ("PaperBall");
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move(){
		// Only follow the ball if the cat is receiving
		if (!ball.IsTowardsCat()) { return; }

		// Determine how far the cat will go.
		// His target is all the way to the ball (ball.x - cat.x)
		// but limited by maxSpeed in either direction
		float distance = Mathf.Clamp(ball.transform.position.x - transform.position.x, -maxSpeed, maxSpeed);

		float targetX = transform.position.x + (distance * Time.deltaTime);
		transform.position = new Vector3(Mathf.Clamp (targetX, -table.transform.lossyScale.x * 0.5f, table.transform.lossyScale.x * 0.5f), transform.position.y, transform.position.z);
	}
}
