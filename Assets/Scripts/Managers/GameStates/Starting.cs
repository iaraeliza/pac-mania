using System.Collections.Generic;
using UnityEngine;

public class Starting : GameMachineState
{
	private float _timer;

	public Starting(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
		: base(gameManager, pacmanLives, allMovableCharacters, ghostHouse)
	{
		Name = GameState.Starting;
	}

	public override void Enter()
	{
		StopAllCharacters();
		_timer = StartupTime;
		GhostHouse.enabled = false;
		base.Enter();
	}

	public override void Update()
	{
		_timer -= Time.deltaTime;

		if (_timer <= 0)
		{
			ChangeState(new Playing(GameManager, PacmanLives, AllMovableCharacters, GhostHouse));
		}
	}

	public override void Exit()
	{
		GameManager.GameStarted();
		GhostHouse.enabled = true;
		base.Exit();
	}
}
