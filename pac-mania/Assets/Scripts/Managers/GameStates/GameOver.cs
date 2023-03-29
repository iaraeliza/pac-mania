using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameOver : GameMachineState
{
	public GameOver(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
		: base(gameManager, pacmanLives, allMovableCharacters, ghostHouse)
	{
		//
	}

	public override void Update()
	{
		if (Input.anyKeyDown)
		{
			SceneManager.LoadScene(0);
		}

		base.Update();
	}
}