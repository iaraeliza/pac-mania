using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private GameMachineState _currentMachineState;

	public event Action OnGameStarted;
	public event Action OnVictory;
	public event Action OnGameOver;

	public void GameStarted()
	{
		OnGameStarted?.Invoke();
	}

	public void Victory()
	{
		OnVictory?.Invoke();
	}

	public void GameOver()
	{
		OnGameOver?.Invoke();
	}

	private void Start()
	{
		var allMovableCharacters = new List<IMovableCharacter>();

		var pacman = GameObject.FindWithTag("Player");
		allMovableCharacters.Add(pacman.GetComponent<PacmanInput>());

		var allGhosts = FindObjectsOfType<GhostAI>();
		allMovableCharacters.AddRange(allGhosts);

		var ghostHouse = FindObjectOfType<GhostHouse>();

		var pacmanLives = pacman.GetComponent<Life>();

		_currentMachineState = new Starting(this, pacmanLives, allMovableCharacters, ghostHouse);
	}

	private void Update()
	{
		_currentMachineState = _currentMachineState.Handle();
	}
}
