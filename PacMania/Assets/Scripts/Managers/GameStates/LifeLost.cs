using System.Collections.Generic;
using UnityEngine;

public class LifeLost : GameMachineState
{
	private float _timer;

	public LifeLost(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
		: base(gameManager, pacmanLives, allMovableCharacters, ghostHouse)
	{
		Name = GameState.LifeLost;
	}

	public override void Enter()
	{
		_timer = LifeLostTimer;
		base.Enter();
	}

	public override void Update()
	{
		_timer -= Time.deltaTime;

		if (_timer <= 0)
		{
			if (PacmanLives.Lives <= 0)
			{
				ChangeState(new Defeat(GameManager, PacmanLives, AllMovableCharacters, GhostHouse));
			}
			else
			{
				ChangeState(new Playing(GameManager, PacmanLives, AllMovableCharacters, GhostHouse));
			}
		}
	}
}
