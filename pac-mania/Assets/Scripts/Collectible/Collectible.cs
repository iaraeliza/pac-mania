using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	public bool IsVictoryCondition;

	public int Score;

	public event Action<int, Collectible> OnCollected;

	private void OnTriggerEnter2D(Collider2D other)
	{
		OnCollected?.Invoke(Score, this);
		Destroy(gameObject);
	}
}
