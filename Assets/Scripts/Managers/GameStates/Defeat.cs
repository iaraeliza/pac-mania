using System.Collections.Generic;

public class Defeat : GameOver
{
	public Defeat(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
		: base(gameManager, pacmanLives, allMovableCharacters, ghostHouse)
	{
		Name = GameState.Defeat;
	}

	public override void Enter()
	{
		GameManager.GameOver();
		base.Enter();
	}
}
