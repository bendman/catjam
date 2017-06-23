using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ballPrefabs;

    void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

    public void SpawnBall()
    {
        GameObject chosenBall = ballPrefabs[Random.Range(0, ballPrefabs.Length)];
        GameObject ballInstance = Instantiate(chosenBall) as GameObject;
        ballInstance.transform.position = transform.position;
    }
}
