using System.Collections.Generic;

public class GameMachineState
{
	public enum GameState
	{
		Starting,
		Playing,
		LifeLost,
		Defeat,
		Victory
	}

	protected enum Event
	{
		Enter, Update, Exit
	}

	public GameState Name;

	public const float StartupTime = 4f;
	public const float LifeLostTimer = 3f;

	protected Event Stage;
	protected GameMachineState NextState;

	protected GameManager GameManager;
	protected Life PacmanLives;
	protected List<IMovableCharacter> AllMovableCharacters;
	protected GhostHouse GhostHouse;

	public GameMachineState(GameManager gameManager, Life pacmanLives, List<IMovableCharacter> allMovableCharacters, GhostHouse ghostHouse)
	{
		GameManager = gameManager;
		PacmanLives = pacmanLives;
		AllMovableCharacters = allMovableCharacters;
		GhostHouse = ghostHouse;
	}

	public virtual void Enter() { Stage = Event.Update; }
	public virtual void Update() { Stage = Event.Update; }
	public virtual void Exit() { Stage = Event.Enter; }

	public GameMachineState Handle()
	{
		switch (Stage)
		{
			case Event.Enter:
				Enter();
				break;

			case Event.Update:
				Update();
				break;

			case Event.Exit:
				Exit();
				return NextState;
		}

		return this;
	}

	protected void ChangeState(GameMachineState nextState)
	{
		NextState = nextState;
		Stage = Event.Exit;
	}

	protected void ResetGameState()
	{
		foreach (var character in AllMovableCharacters)
		{
			character.ResetPosition();
		}

		GhostHouse.Reset();
	}

	protected void StartAllCharacters()
	{
		foreach (var character in AllMovableCharacters)
		{
			character.StartMoving();
		}
	}

	protected void StopAllCharacters()
	{
		foreach (var character in AllMovableCharacters)
		{
			character.StopMoving();
		}
	}
}
