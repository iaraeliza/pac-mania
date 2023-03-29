using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	private int _currentScore;

	private int _highScore;

	private int _currentGhostCaughtsCount;
	private GhostScoreData _ghostScoreData;

	public int CurrentScore { get => _currentScore; }

	public int HighScore { get => _highScore; }

	public event Action<int> OnScoreChanged;
	public event Action<int> OnHighScoreChanged;
	public event Action<int, GhostAI> OnShowGhostScore;

	public void EnergizerActivated(GhostScoreData ghostScoreData)
	{
		_ghostScoreData = ghostScoreData;
		_currentGhostCaughtsCount = 0;
	}

	private void Awake()
	{
		_highScore = PlayerPrefs.GetInt("high-score", 0);
	}

	private void Start()
	{
		var allCollectibles = FindObjectsOfType<Collectible>();
		foreach (var collectible in allCollectibles)
		{
			collectible.OnCollected += Collectible_OnCollected;
		}

		var allGhosts = FindObjectsOfType<GhostAI>();
		foreach (var ghost in allGhosts)
		{
			ghost.OnGhostCaught += GhostAI_OnGhostCaught;
		}
	}

	private void GhostAI_OnGhostCaught(GhostAI ghost)
	{
		int score = _ghostScoreData.GhostScore + _ghostScoreData.GhostScoreIncrement * _currentGhostCaughtsCount;

		AddScore(score);

		_currentGhostCaughtsCount++;

		OnShowGhostScore?.Invoke(score, ghost);
	}

	private void Collectible_OnCollected(int score, Collectible collectible)
	{
		AddScore(score);

		collectible.OnCollected -= Collectible_OnCollected;
	}

	private void AddScore(int score)
	{
		_currentScore += score;
		OnScoreChanged?.Invoke(_currentScore);

		if (_currentScore >= _highScore)
		{
			_highScore = _currentScore;
			OnHighScoreChanged?.Invoke(_highScore);
		}
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetInt("high-score", _highScore);
	}
}
