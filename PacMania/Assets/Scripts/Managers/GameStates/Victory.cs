using System.Collections.Generic;

public class Victory : GameOver
{
	public Victory(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
		: base(gameManager, pacmanLives, allMovableCharacters, ghostHouse)
	{
		Name = GameState.Victory;
	}

	public override void Enter()
	{
		GameManager.Victory();
		base.Enter();
	}
}
