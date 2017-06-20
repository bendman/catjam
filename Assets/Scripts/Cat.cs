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
		float direction = 1.0f;

		if ((Mathf.Abs(this.transform.position.x - ball.transform.position.x) < followThreshold) || !ball.IsTowardsCat()) {
			return;
		}

		if (this.transform.position.x > ball.transform.position.x) {
			direction = -1.0f;
		}

		Debug.Log (direction + "  this " + transform.position.x + " ball " + ball.transform.position.x);

		float targetX = transform.position.x + (direction * maxSpeed * Time.deltaTime);
		transform.position = new Vector3(Mathf.Clamp (targetX, -table.transform.lossyScale.x * 0.5f, table.transform.lossyScale.x * 0.5f), transform.position.y, transform.position.z);
	}
}
