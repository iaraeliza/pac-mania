using UnityEngine;

public abstract class GhostAIMachineState
{
	public enum GhostAIState
	{
		InHouse,
		LeavingHouse,
		Chasing,
		Vulnerable,
		Defeated
	}

	protected enum Event
	{
		Enter, Update, Exit
	}

	public GhostAIState Name;

	protected const float VulnerabilityEndingTime = 3f;

	protected Event Stage;
	protected GhostAIMachineState NextState;

	protected GhostAI GhostAI;
	protected GhostMove GhostMove;
	protected Transform Pacman;

	public GhostAIMachineState(GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
	{
		GhostAI = ghostAI;
		GhostMove = ghostMove;
		Pacman = pacman;
	}

	public virtual void Enter() { Stage = Event.Update; }
	public virtual void Update() { Stage = Event.Update; }
	public virtual void Exit() { Stage = Event.Enter; }

	public GhostAIMachineState Handle()
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

	public void ForceChangeState(GhostAIMachineState nextState)
	{
		ChangeState(nextState);
	}

	protected void ChangeState(GhostAIMachineState nextState)
	{
		NextState = nextState;
		Stage = Event.Exit;
	}
}
