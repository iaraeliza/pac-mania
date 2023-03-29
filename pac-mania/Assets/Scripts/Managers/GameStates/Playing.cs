using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Playing : GameMachineState
{
	private readonly List<Collectible> _remainingCollectibles;

	public Playing(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
		: base(gameManager, pacmanLives, allMovableCharacters, ghostHouse)
	{
		Name = GameState.Playing;
		pacmanLives.OnLifeRemoved += PacmanLives_OnLifeRemoved;

		_remainingCollectibles = Object.FindObjectsOfType<Collectible>().ToList();

		foreach (var collectible in _remainingCollectibles)
		{
			collectible.OnCollected += Collectible_OnCollected;
		}
	}

	public override void Enter()
	{
		ResetGameState();
		StartAllCharacters();
		base.Enter();
	}

	private void PacmanLives_OnLifeRemoved(int _)
	{
		ChangeState(new LifeLost(GameManager, PacmanLives, AllMovableCharacters, GhostHouse));
	}

	private void Collectible_OnCollected(int _, Collectible collectedCollectible)
	{
		_remainingCollectibles.Remove(collectedCollectible);

		if (_remainingCollectibles.Count <= 0)
		{
			ChangeState(new Victory(GameManager, PacmanLives, AllMovableCharacters, GhostHouse));
		}

		collectedCollectible.OnCollected -= Collectible_OnCollected;
	}

	public override void Exit()
	{
		StopAllCharacters();

		foreach (var collectible in _remainingCollectibles)
		{
			collectible.OnCollected -= Collectible_OnCollected;
		}

		base.Exit();
	}
}
